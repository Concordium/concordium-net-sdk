use anyhow::Result;
use concordium_contracts_common::{
    from_bytes,
    schema::{Type, VersionedModuleSchema, VersionedSchemaError},
    schema_json, Cursor,
};
use serde_json::to_vec;
use std::{ffi::CStr, os::raw::c_char};
use thiserror::Error;

pub type JsonString = String;

/// Callback allowing the callee to copy results from an array into their
/// environment. The callee is expected to handle all errors.
type ResultCallback = extern "C" fn(*const u8, i32) -> ();

#[repr(C)]
pub struct FFIByteOption {
    pub t:       u8,
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
/// * 'schema' - Module schema
/// * 'schem_version' - Optional schema version
/// * 'callback' - Callback which can be used to set resulting output
///
/// # Returns
///
/// If the call succeeded or not.
///
/// # Safety
///
/// The same caveats as for [`std::slice::from_raw_parts`] apply in
/// relation to safety and lifetimes.
#[no_mangle]
pub unsafe extern "C" fn schema_display(
    schema_ptr: *const u8,
    schema_size: i32,
    schema_version: FFIByteOption,
    callback: ResultCallback,
) -> u16 {
    let schema = std::slice::from_raw_parts(schema_ptr, schema_size as usize);
    assign_result(callback, || {
        schema_display_aux(schema, schema_version.into_option())
    })
}

/// Get contract receive parameters in a human interpretable form.
///
/// Receive parameters are those given to a contract entrypoint on a update
/// call.
///
/// # Arguments
///
/// * 'schema' - Module schema
/// * 'schem_version' - Optional schema version
/// * 'contract_name' - Contract name
/// * 'entrypoint' - Entrypoint of contract
/// * 'value' - Receive parameters
/// * 'callback' - Callback which can be used to set resulting output
///
/// # Returns
///
/// If the call succeeded or not.
///
/// # Safety
///
/// The same caveats as for [`std::slice::from_raw_parts`] apply in
/// relation to safety and lifetimes.
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
) -> u16 {
    assign_result(callback, || {
        let schema = std::slice::from_raw_parts(schema_ptr, schema_size as usize);
        let contract_name_str = get_str_from_pointer(contract_name)?;
        let entrypoint_str = get_str_from_pointer(entrypoint)?;
        let value = std::slice::from_raw_parts(value_ptr, value_size as usize);

        get_receive_contract_parameter_aux(
            schema,
            schema_version.into_option(),
            contract_name_str,
            entrypoint_str,
            value,
        )
    })
}

/// Get contract event in a human interpretable form.
///
/// # Arguments
///
/// * 'schema' - Module schema
/// * 'schem_version' - Optinal schema version
/// * 'contract_name' - Contract name
/// * 'value' - Contract event
/// * 'callback' - Callback which can be used to set resulting output
///
/// # Returns
///
/// If the call succeeded or not.
///
/// # Safety
///
/// The same caveats as for [`std::slice::from_raw_parts`] apply in
/// relation to safety and lifetimes.
#[no_mangle]
pub unsafe extern "C" fn get_event_contract(
    schema_ptr: *const u8,
    schema_size: i32,
    schema_version: FFIByteOption,
    contract_name: *const c_char,
    value_ptr: *const u8,
    value_size: i32,
    callback: ResultCallback,
) -> u16 {
    assign_result(callback, || {
        let schema = std::slice::from_raw_parts(schema_ptr, schema_size as usize);
        let contract_name_str = get_str_from_pointer(contract_name)?;
        let value = std::slice::from_raw_parts(value_ptr, value_size as usize);

        get_event_contract_aux(
            schema,
            schema_version.into_option(),
            contract_name_str,
            value,
        )
    })
}

/// Construct smart contract receive parameter from a JSON string and a smart
/// contract schema.
///
/// # Arguments
///
/// * 'schema_ptr' - Pointer to smart contract module schema.
/// * 'schema_size' - The byte size of the smart contract module schema.
/// * 'schema_version' - Version of the smart contract module schema (Optional).
/// * 'contract_name' - Contract name provided as a null terminated string.
/// * 'function_name' - Receive function name provided as a null terminated
///   string.
/// * 'json_ptr' - Pointer to the UTF8 encoded JSON parameter.
/// * 'json_size' - The byte size of the encoded JSON parameter.
/// * 'callback' - Callback which can be used to set resulting output
///
/// # Returns
///
/// 0 if the call succeeded otherwise the return value corresponds to some error
/// code.
///
/// # Safety
///
/// Every pointer provided as an argument is assumed to be alive for the
/// duration of the call.
#[no_mangle]
pub unsafe extern "C" fn into_receive_parameter(
    schema_ptr: *const u8,
    schema_size: i32,
    schema_version: FFIByteOption,
    contract_name: *const c_char,
    function_name: *const c_char,
    json_ptr: *const u8,
    json_size: i32,
    callback: ResultCallback,
) -> u16 {
    assign_result(callback, || {
        let schema = std::slice::from_raw_parts(schema_ptr, schema_size as usize);
        let contract_name_str = get_str_from_pointer(contract_name)?;
        let function_name_str = get_str_from_pointer(function_name)?;
        let json_slice = std::slice::from_raw_parts(json_ptr, json_size as usize);
        let module_schema = VersionedModuleSchema::new(schema, &schema_version.into_option())?;
        let parameter_schema_type =
            module_schema.get_receive_param_schema(contract_name_str, function_name_str)?;
        let json_value: serde_json::Value = serde_json::from_slice(json_slice)?;
        Ok(parameter_schema_type.serial_value(&json_value)?)
    })
}

/// Construct smart contract init parameter from a JSON string and a smart
/// contract schema.
///
/// # Arguments
///
/// * 'schema_ptr' - Pointer to smart contract module schema.
/// * 'schema_size' - The byte size of the smart contract module schema.
/// * 'schema_version' - Version of the smart contract module schema (Optional).
/// * 'contract_name' - Contract name provided as a null terminated string.
/// * 'json_ptr' - Pointer to the UTF8 encoded JSON parameter.
/// * 'json_size' - The byte size of the encoded JSON parameter.
/// * 'callback' - Callback which can be used to set resulting output
///
/// # Returns
///
/// 0 if the call succeeded otherwise the return value corresponds to some error
/// code.
///
/// # Safety
///
/// Every pointer provided as an argument is assumed to be alive for the
/// duration of the call.
#[no_mangle]
pub unsafe extern "C" fn into_init_parameter(
    schema_ptr: *const u8,
    schema_size: i32,
    schema_version: FFIByteOption,
    contract_name: *const c_char,
    json_ptr: *const u8,
    json_size: i32,
    callback: ResultCallback,
) -> u16 {
    assign_result(callback, || {
        let schema = std::slice::from_raw_parts(schema_ptr, schema_size as usize);
        let contract_name_str = get_str_from_pointer(contract_name)?;
        let json_slice = std::slice::from_raw_parts(json_ptr, json_size as usize);
        let module_schema = VersionedModuleSchema::new(schema, &schema_version.into_option())?;
        let parameter_schema_type = module_schema.get_init_param_schema(contract_name_str)?;
        let json_value: serde_json::Value = serde_json::from_slice(json_slice)?;
        Ok(parameter_schema_type.serial_value(&json_value)?)
    })
}

/// Convert some JSON representation into bytes using a smart contract schema
/// type.
///
/// # Arguments
///
/// * 'schema_type_ptr' - Pointer to smart contract schema type.
/// * 'schema_type_size' - The byte size of the smart contract schema type.
/// * 'json_ptr' - Pointer to the UTF8 encoded JSON parameter.
/// * 'json_size' - The byte size of the encoded JSON parameter.
/// * 'callback' - Callback which can be used to set resulting output
///
/// # Returns
///
/// 0 if the call succeeded otherwise the return value corresponds to some error
/// code.
///
/// # Safety
///
/// Every pointer provided as an argument is assumed to be alive for the
/// duration of the call.
#[no_mangle]
pub unsafe extern "C" fn schema_json_to_bytes(
    schema_type_ptr: *const u8,
    schema_type_size: i32,
    json_ptr: *const u8,
    json_size: i32,
    callback: ResultCallback,
) -> u16 {
    assign_result(callback, || {
        let schema_type_bytes =
            std::slice::from_raw_parts(schema_type_ptr, schema_type_size as usize);
        let json_slice = std::slice::from_raw_parts(json_ptr, json_size as usize);
        let parameter_schema_type: Type =
            from_bytes(&schema_type_bytes).map_err(|_| FFIError::ParseSchemaType)?;
        let json_value: serde_json::Value = serde_json::from_slice(json_slice)?;
        Ok(parameter_schema_type.serial_value(&json_value)?)
    })
}

/// Compute result using the provided callback f, convert it into a C string and
/// assign it to the provided target.
///
/// # Arguments
///
/// * 'callback' - callback enabling the callee to copy the result.
/// * 'f' - callback function, which generates result.
///
/// # Returns
///
/// A boolean, that indicates whether the computation was successful or not.
fn assign_result<F: FnOnce() -> Result<Vec<u8>, FFIError>>(callback: ResultCallback, f: F) -> u16 {
    match f() {
        Ok(output) => {
            let out_lenght = output.len() as i32;
            let ptr = output.as_ptr();
            callback(ptr, out_lenght);
            0
        }
        Err(e) => {
            let error = e.to_string().into_bytes();
            let error_length = error.len() as i32;
            let ptr = error.as_ptr();
            callback(ptr, error_length);
            e.to_int()
        }
    }
}

fn get_receive_contract_parameter_aux(
    schema: &[u8],
    schema_version: Option<u8>,
    contract_name: &str,
    entrypoint: &str,
    value: &[u8],
) -> Result<Vec<u8>, FFIError> {
    let module_schema = VersionedModuleSchema::new(schema, &schema_version)?;
    let parameter_type = module_schema.get_receive_param_schema(contract_name, entrypoint)?;
    let deserialized = deserialize_type_value(value, &parameter_type)?;
    Ok(deserialized)
}

fn schema_display_aux(schema: &[u8], schema_version: Option<u8>) -> Result<Vec<u8>, FFIError> {
    let display = VersionedModuleSchema::new(schema, &schema_version)?;
    Ok(display.to_string().into_bytes())
}

#[derive(Error, Debug)]
enum FFIError {
    #[error("{}", .0.display(true))]
    ToJsonError(#[from] schema_json::ToJsonError),
    #[error("error when using serde")]
    SerdeJsonError(#[from] serde_json::Error),
    #[error("encountered string which wasn't utf8 encoded")]
    Utf8Error(#[from] std::str::Utf8Error),
    #[error(transparent)]
    VersionedSchemaError(#[from] VersionedSchemaError),
    #[error(transparent)]
    FromJsonError(#[from] schema_json::JsonError),
    #[error("error parsing the schema type")]
    ParseSchemaType,
}

impl FFIError {
    /// The enumeration starts a 1 since return value 0 indicating a successfull
    /// FFI call.
    fn to_int(&self) -> u16 {
        match self {
            FFIError::ToJsonError(_) => 1,
            FFIError::SerdeJsonError(_) => 2,
            FFIError::Utf8Error(_) => 3,
            FFIError::VersionedSchemaError(schema_error) => match schema_error {
                VersionedSchemaError::ParseError => 4,
                VersionedSchemaError::MissingSchemaVersion => 5,
                VersionedSchemaError::InvalidSchemaVersion => 6,
                VersionedSchemaError::NoContractInModule => 7,
                VersionedSchemaError::NoReceiveInContract => 8,
                VersionedSchemaError::NoInitInContract => 9,
                VersionedSchemaError::NoParamsInReceive => 10,
                VersionedSchemaError::NoParamsInInit => 11,
                VersionedSchemaError::NoErrorInReceive => 12,
                VersionedSchemaError::NoErrorInInit => 13,
                VersionedSchemaError::ErrorNotSupported => 14,
                VersionedSchemaError::NoReturnValueInReceive => 15,
                VersionedSchemaError::ReturnValueNotSupported => 16,
                VersionedSchemaError::NoEventInContract => 17,
                VersionedSchemaError::EventNotSupported => 18,
            },
            FFIError::FromJsonError(_) => 19,
            FFIError::ParseSchemaType => 20,
        }
    }
}

fn get_event_contract_aux(
    schema: &[u8],
    schema_version: Option<u8>,
    contract_name: &str,
    value: &[u8],
) -> Result<Vec<u8>, FFIError> {
    let module_schema = VersionedModuleSchema::new(schema, &schema_version)?;
    let parameter_type = module_schema.get_event_schema(contract_name)?;
    let deserialized = deserialize_type_value(value, &parameter_type)?;
    Ok(deserialized)
}

fn deserialize_type_value(value: &[u8], value_type: &Type) -> Result<Vec<u8>, FFIError> {
    let mut cursor = Cursor::new(value);
    let v = value_type.to_json(&mut cursor)?;
    Ok(to_vec(&v)?)
}

/// The provided raw pointer [`c_char`] must be a [`std::ffi::CString`].
/// The content of the pointer [`c_char`] must not be mutated for the duration
/// of lifetime 'a.
fn get_str_from_pointer<'a>(input: *const c_char) -> Result<&'a str, FFIError> {
    let c_str: &CStr = unsafe { CStr::from_ptr(input) };
    Ok(c_str.to_str()?)
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
        assert_eq!(String::from_utf8(display)?, expected);
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
        assert_eq!(String::from_utf8(display)?, expected);
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
        assert_eq!(String::from_utf8(display)?, expected);
        Ok(())
    }
}
