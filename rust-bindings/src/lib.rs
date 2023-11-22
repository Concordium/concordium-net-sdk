use std::{
    ffi::{CStr, CString},
    os::raw::c_char,
};

use anyhow::{anyhow, Result};
use concordium_contracts_common::{
    schema::{Type, VersionedModuleSchema},
    Cursor,
};
use serde_json::to_string;

pub type JsonString = String;

type ResultCallback = unsafe extern "C" fn(*mut i8) -> ();

#[repr(C)]
pub struct FFIByteOption {
    pub t: u8,
    pub is_some: u8,
}

impl FFIByteOption {
    pub fn into_option(self) -> Option<u8> {
        match self.is_some {
            1 => Option::Some(self.t),
            _ => Option::None,
        }
    }
}

/// Get module schema in a human interpretable form.
///
/// # Arguments
///
/// * 'schema' - Module schema in hexadecimal
/// * 'schem_version' - Optional schema version
/// * 'result' - Parsed schema if the call succeeded or the error message in case of failure.
///
/// # Returns
///
/// If the call succeeded or not.
///
/// # Safety
///
/// The same caveats as for [`std::slice::from_raw_parts`] apply in
/// relation to safety and lifetimes.
///
/// The callback given must also ensure safety and handle any errors
/// within that function.
/// Rust compiler.
#[no_mangle]
pub unsafe extern "C" fn schema_display(
    schema_ptr: *const u8,
    schema_size: i32,
    schema_version: FFIByteOption,
    callback: ResultCallback,
) -> bool {
    let schema = slice_from_ptr(schema_ptr, schema_size as usize);
    assign_result(callback, || {
        schema_display_aux(schema, schema_version.into_option())
    })
}

/// Get contract receive parameters in a human interpretable form.
///
/// Receive parameters are those given to a contract entrypoint on a update call.
///
/// # Arguments
///
/// * 'schema' - Module schema in hexadecimal
/// * 'schem_version' - Optional schema version
/// * 'contract_name' - Contract name
/// * 'entrypoint' - Entrypoint of contract
/// * 'value' - Receive parameters in hexadecimal
/// * 'result' - Parsed receive parameters if the call succeeded or the error message in case of failure.
///
/// # Returns
///
/// If the call succeeded or not.
///
/// # Safety
///
/// The same caveats as for [`std::slice::from_raw_parts`] apply in
/// relation to safety and lifetimes.
///
/// The callback given must also ensure safety and handle any errors
/// within that function.
#[no_mangle]
pub unsafe extern "C" fn get_receive_contract_parameter(
    schema_ptr: *const u8,
    schema_size: i32,
    schema_version: FFIByteOption,
    contract_name: *const c_char,
    entrypoint: *const c_char,
    value_ptr: *const u8,
    value_size: i32,
    callback: ResultCallback,
) -> bool {
    assign_result(callback, || {
        let schema = slice_from_ptr(schema_ptr, schema_size as usize);
        let contract_name_str = get_str_from_pointer(contract_name)?;
        let entrypoint_str = get_str_from_pointer(entrypoint)?;
        let value = slice_from_ptr(value_ptr, value_size as usize);

        get_receive_contract_parameter_aux(
            schema,
            schema_version.into_option(),
            &contract_name_str,
            &entrypoint_str,
            value,
        )
    })
}

/// Get contract event in a human interpretable form.
///
/// # Arguments
///
/// * 'schema' - Module schema in hexadecimal
/// * 'schem_version' - Optinal schema version
/// * 'contract_name' - Contract name
/// * 'value' - Contract event in hexadecimal
/// * 'result' - Parsed contract event if the call succeeded or the error message in case of failure.
///
/// # Returns
///
/// If the call succeeded or not.
///
/// # Safety
///
/// The same caveats as for [`std::slice::from_raw_parts`] apply in
/// relation to safety and lifetimes.
///
/// The callback given must also ensure safety and handle any errors
/// within that function.
#[no_mangle]
pub unsafe extern "C" fn get_event_contract(
    schema_ptr: *const u8,
    schema_size: i32,
    schema_version: FFIByteOption,
    contract_name: *const c_char,
    value_ptr: *const u8,
    value_size: i32,
    callback: ResultCallback,
) -> bool {
    assign_result(callback, || {
        let schema = slice_from_ptr(schema_ptr, schema_size as usize);
        let contract_name_str = get_str_from_pointer(contract_name)?;
        let value = slice_from_ptr(value_ptr, value_size as usize);

        get_event_contract_aux(
            schema,
            schema_version.into_option(),
            &contract_name_str,
            value,
        )
    })
}

/// Compute result using the provided callback f, convert it into a C string and assign it to the provided target.
///
/// # Arguments
///
/// * 'target' - Pointer to a C String, which will be assigned the result / error message of f.
/// * 'f' - callback function, which result should be assigned to target.
///
/// # Returns
///
/// A boolean, that indicates whether the computation was successful or not.
///
/// # Safety
///
/// The callback given must also ensure safety and handle any errors
/// within that function.
unsafe fn assign_result<F: FnOnce() -> Result<T>, T: ToString>(
    callback: ResultCallback,
    f: F,
) -> bool {
    match f() {
        Ok(output) => match CString::new(output.to_string()) {
            Ok(output_string) => {
                let target = output_string.into_raw();
                callback(target);
                drop(CString::from_raw(target));
                true
            }
            Err(e) => {
                let error_string = match CString::new(e.to_string()) {
                    Ok(e_string) => e_string,
                    Err(_) => CString::new("NulError when creating CString").unwrap(),
                };
                let target = error_string.into_raw();
                callback(target);
                drop(CString::from_raw(target));
                false
            }
        },
        Err(e) => {
            let error_string = match CString::new(e.to_string()) {
                Ok(e_string) => e_string,
                Err(_) => CString::new("NulError when creating CString").unwrap(),
            };
            let target = error_string.into_raw();
            callback(target);
            drop(CString::from_raw(target));
            false
        }
    }
}

pub fn get_receive_contract_parameter_aux(
    schema: &[u8],
    schema_version: Option<u8>,
    contract_name: &str,
    entrypoint: &str,
    value: &[u8],
) -> Result<String> {
    let module_schema = VersionedModuleSchema::new(schema, &schema_version)?;
    let parameter_type = module_schema.get_receive_param_schema(contract_name, entrypoint)?;
    let deserialized = deserialize_type_value(value, &parameter_type, true)?;
    Ok(deserialized)
}

unsafe fn slice_from_ptr<'a, T>(data: *const T, size: usize) -> &'a [T] {
    std::slice::from_raw_parts(data, size)
}

fn schema_display_aux(schema: &[u8], schema_version: Option<u8>) -> Result<String> {
    let display = VersionedModuleSchema::new(schema, &schema_version)?;
    Ok(display.to_string())
}

fn get_event_contract_aux(
    schema: &[u8],
    schema_version: Option<u8>,
    contract_name: &str,
    value: &[u8],
) -> Result<String> {
    let module_schema = VersionedModuleSchema::new(schema, &schema_version)?;
    let parameter_type = module_schema.get_event_schema(contract_name)?;
    let deserialized = deserialize_type_value(value, &parameter_type, true)?;
    Ok(deserialized)
}

fn deserialize_type_value(
    value: &[u8],
    value_type: &Type,
    verbose_error_message: bool,
) -> Result<String> {
    let mut cursor = Cursor::new(value);
    match value_type.to_json(&mut cursor) {
        Ok(v) => Ok(to_string(&v)?),
        Err(e) => Err(anyhow!("{}", e.display(verbose_error_message))),
    }
}

fn get_str_from_pointer(input: *const c_char) -> Result<String> {
    let c_str: &CStr = unsafe { CStr::from_ptr(input) };
    let str_slice: &str = c_str.to_str()?;
    Ok(str_slice.to_string())
}

#[cfg(test)]
mod test {
    use super::*;
    use std::fs;

    #[test]
    fn test_display_event() -> Result<()> {
        // Arrange
        let expected = r#"{"Mint":{"amount":"1000000","owner":{"Account":["3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV"]},"token_id":""}}"#;
        let schema_version = Option::None;
        let schema = fs::read_to_string("./test-data/cis2_wCCD_sub")?;
        let contract_name = "cis2_wCCD";
        let message =
            "fe00c0843d005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc79";

        // Act
        let display = get_event_contract_aux(
            &hex::decode(schema)?,
            schema_version,
            contract_name,
            &hex::decode(message)?,
        )?;

        // Assert
        assert_eq!(display, expected);
        Ok(())
    }

    #[test]
    fn test_display_receive_param() -> Result<()> {
        // Arrange
        let expected = r#"{"data":"","to":{"Account":["3fpkgmKcGDKGgsDhUQEBAQXbFZJQw97JmbuhzmvujYuG1sQxtV"]}}"#;
        let schema_version = Option::None;
        let schema = fs::read_to_string("./test-data/cis2_wCCD_sub")?;
        let contract_name = "cis2_wCCD";
        let entrypoint = "wrap";
        let message = "005f8b99a3ea8089002291fd646554848b00e7a0cd934e5bad6e6e93a4d4f4dc790000";

        // Act
        let display = get_receive_contract_parameter_aux(
            &hex::decode(schema)?,
            schema_version,
            contract_name,
            entrypoint,
            &hex::decode(message)?,
        )?;

        // Assert
        assert_eq!(display, expected);
        Ok(())
    }

    #[test]
    fn test_display_module_schema() -> Result<()> {
        // Arrange
        let expected = r#"Contract: TestContract
  Event:
    {
      "Enum": [
        {
          "Foo": []
        },
        {
          "Bar": []
        }
      ]
    }
"#;
        let schema_version = Option::None;
        let schema = "ffff03010000000c00000054657374436f6e7472616374000000000001150200000003000000466f6f020300000042617202".to_string();

        // Act
        let display = schema_display_aux(&hex::decode(schema)?, schema_version)?;

        // Assert
        assert_eq!(display, expected);
        Ok(())
    }
}
