using System.Buffers.Binary;
using System.Collections;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ConcordiumNetSdk.SchemaSerialization.Types;
using ConcordiumNetSdk.Types;
using Type = ConcordiumNetSdk.SchemaSerialization.Types.Type;

namespace ConcordiumNetSdk.SchemaSerialization;

/// <summary>
/// Represents a smart contract parameters serializer.
/// </summary>
public static class ContractParametersSerializer
{
    /// <summary>
    /// Serializes smart contract parameters to byte format.
    /// </summary>
    /// <param name="paramType">the parameter type.</param>
    /// <param name="paramArgument">the parameter argument.</param>
    /// <returns><see cref="T:byte[]"/> - serialized smart contract parameter in byte format.</returns>
    public static byte[] Serialize(Type? paramType, dynamic? paramArgument)
    {
        ParameterType typeTag = paramType?.TypeTag ?? ParameterType.Unit;
        switch (typeTag)
        {
            case ParameterType.U8:
            {
                if (paramArgument is not byte value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.U8)} require value of type {nameof(Byte)}.");
                return SerializeUInt8(value);
            }
            case ParameterType.U16:
            {
                if (paramArgument is not ushort value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.U16)} require value of type {nameof(UInt16)}.");
                return SerializeUInt16(value, true);
            }
            case ParameterType.U32:
            {
                if (paramArgument is not uint value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.U32)} require value of type {nameof(UInt32)}.");
                return SerializeUInt32(value, true);
            }
            case ParameterType.U64:
            {
                if (paramArgument is not ulong value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.U64)} require value of type {nameof(UInt64)}.");
                return SerializeUInt64(value, true);
            }
            case ParameterType.U128:
            {
                if (paramArgument is not BigInteger value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.U128)} require value of type {nameof(BigInteger)}.");
                return SerializeUInt128Le(value);
            }
            case ParameterType.I8:
            {
                if (paramArgument is not sbyte value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.I8)} require value of type {nameof(SByte)}.");
                return SerializeInt8(value);
            }
            case ParameterType.I16:
            {
                if (paramArgument is not short value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.I16)} require value of type {nameof(Int16)}.");
                return SerializeInt16(value, true);
            }
            case ParameterType.I32:
            {
                if (paramArgument is not int value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.I32)} require value of type {nameof(Int32)}.");
                return SerializeInt32(value, true);
            }
            case ParameterType.I64:
            {
                if (paramArgument is not long value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.I64)} require value of type {nameof(Int64)}.");
                return SerializeInt64(value, true);
            }
            case ParameterType.I128:
            {
                if (paramArgument is not BigInteger value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.I128)} require value of type {nameof(BigInteger)}.");
                return SerializeInt128Le(value);
            }
            case ParameterType.Bool:
            {
                if (paramArgument is not bool value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Bool)} require value of type {nameof(Boolean)}.");
                return SerializeBool(value);
            }
            case ParameterType.String:
            {
                if (paramArgument is not string value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.String)} require value of type {nameof(String)}.");
                StringType stringType = paramType as StringType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.String)}. Expected {nameof(StringType)}.");
                return SerializeString(stringType, value);
            }
            case ParameterType.Array:
            {
                if (paramArgument is not Array value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Array)} require value of type {nameof(Array)}.");
                ArrayType arrayType = paramType as ArrayType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.Array)}. Expected {nameof(ArrayType)}.");
                return SerializeArray(arrayType, value);
            }
            case ParameterType.Struct:
            {
                if (paramArgument is null) throw new NullReferenceException($"Parameter type {nameof(ParameterType.Struct)} value can not be null.");
                StructType structType = paramType as StructType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.Struct)}. Expected {nameof(StructType)}.");
                return SerializeStruct(structType, paramArgument);
            }
            case ParameterType.AccountAddress:
            {
                if (paramArgument is not AccountAddress accountAddress) throw new InvalidCastException($"Parameter type {nameof(ParameterType.AccountAddress)} require value of type {nameof(AccountAddress)}.");
                return accountAddress.AsBytes;
            }
            case ParameterType.Amount:
            {
                if (paramArgument is not CcdAmount ccdAmount) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Amount)} require value of type {nameof(CcdAmount)}.");
                return ccdAmount.SerializeToBytes(true);
            }
            case ParameterType.Timestamp:
            {
                if (paramArgument is not DateTime timestamp) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Timestamp)} require value of type {nameof(DateTime)}.");
                return SerializeInt64(timestamp.ToBinary(), true);
            }
            case ParameterType.Duration:
            {
                if (paramArgument is not string value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Duration)} require value in {nameof(String)} format.");
                ulong duration = GetMilliSeconds(value);
                return SerializeUInt64(duration, true);
            }
            case ParameterType.ContractAddress:
            {
                if (paramArgument is not ContractAddress contractAddress) throw new NullReferenceException($"Parameter type {nameof(ParameterType.ContractAddress)} require value of type {nameof(ContractAddress)}.");
                return contractAddress.SerializeToBytes(true);
            }
            case ParameterType.Unit:
            {
                return Array.Empty<byte>();
            }
            case ParameterType.List:
            {
                if (paramArgument is not IList value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.List)} require value of type {nameof(IList)}.");
                ListType listType = paramType as ListType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.List)}. Expected {nameof(ListType)}.");
                return SerializeListOrSet(listType, value);
            }
            case ParameterType.Set:
            {
                if (paramArgument is not IList value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Set)} require value of type {nameof(IList)}.");
                ListType listType = paramType as ListType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.Set)}. Expected {nameof(ListType)}.");
                return SerializeListOrSet(listType, value);
            }
            case ParameterType.ContractName:
            {
                if (paramArgument is null) throw new NullReferenceException($"Parameter type {nameof(ParameterType.ContractName)} value can not be null.");
                StringType stringType = paramType as StringType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.ContractName)}. Expected {nameof(StringType)}.");
                return SerializeContractName(stringType, paramArgument);
            }
            case ParameterType.ReceiveName:
            {
                if (paramArgument is null) throw new NullReferenceException($"Parameter type {nameof(ParameterType.ReceiveName)} value can not be null.");
                StringType stringType = paramType as StringType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.ReceiveName)}. Expected {nameof(StringType)}.");
                return SerializeReceiveName(stringType, paramArgument);
            }
            case ParameterType.Pair:
            {
                if (paramArgument is not ArrayList value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Pair)} require value of type {nameof(ArrayList)}.");
                PairType pairType = paramType as PairType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.Pair)}. Expected {nameof(PairType)}.");
                return SerializePairType(pairType, value);
            }
            case ParameterType.Map:
            {
                if (paramArgument is not IDictionary value) throw new InvalidCastException($"Parameter type {nameof(ParameterType.Map)} require value of type {nameof(IDictionary)}.");
                MapType mapType = paramType as MapType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.Map)}. Expected {nameof(MapType)}.");
                return SerializeMapType(mapType, value);
            }
            case ParameterType.Enum:
            {
                if (paramArgument is null) throw new NullReferenceException($"Parameter type {nameof(ParameterType.Enum)} value can not be null.");
                EnumType enumType = paramType as EnumType ?? throw new InvalidCastException($"Invalid param type {nameof(ParameterType.Enum)}. Expected {nameof(EnumType)}.");
                return SerializeEnumType(enumType, paramArgument);
            }
            default:
            {
                throw new IndexOutOfRangeException("Type is not supported currently.");
            }
        }
    }

    private static byte[] SerializeUInt8(byte value)
    {
        return new[] {value};
    }

    private static byte[] SerializeInt8(sbyte value)
    {
        return new[] {(byte) value};
    }

    private static byte[] SerializeUInt16(ushort value, bool useLittleEndian = false)
    {
        byte[] bytes = new byte[2];
        if (useLittleEndian)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        }
        return bytes;
    }

    private static byte[] SerializeInt16(short value, bool useLittleEndian = false)
    {
        byte[] bytes = new byte[2];
        if (useLittleEndian)
        {
            BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteInt16BigEndian(bytes, value);
        }
        return bytes;
    }

    private static byte[] SerializeUInt32(uint value, bool useLittleEndian = false)
    {
        byte[] bytes = new byte[4];
        if (useLittleEndian)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        }
        return bytes;
    }

    private static byte[] SerializeInt32(int value, bool useLittleEndian = false)
    {
        byte[] bytes = new byte[4];
        if (useLittleEndian)
        {
            BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteInt32BigEndian(bytes, value);
        }
        return bytes;
    }

    private static byte[] SerializeUInt64(ulong value, bool useLittleEndian = false)
    {
        byte[] bytes = new byte[8];
        if (useLittleEndian)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        }
        return bytes;
    }

    private static byte[] SerializeInt64(long value, bool useLittleEndian = false)
    {
        byte[] bytes = new byte[8];
        if (useLittleEndian)
        {
            BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
        }
        else
        {
            BinaryPrimitives.WriteInt64BigEndian(bytes, value);
        }
        return bytes;
    }

    private static byte[] SerializeUInt128Le(BigInteger value)
    {
        BigInteger maxValue = BigInteger.Parse("340282366920938463463374607431768211455");
        int minValue = 0;
        if (value > maxValue || value < minValue) throw new InvalidDataException($"The {nameof(ParameterType.U128)} value has to be a 128 bit unsigned integer but it was: '{value}'.");
        return value.ToByteArray(true);
    }

    private static byte[] SerializeInt128Le(BigInteger value)
    {
        BigInteger maxValue = BigInteger.Parse("-170141183460469231731687303715884105728");
        BigInteger minValue = BigInteger.Parse("170141183460469231731687303715884105727");
        if (value < maxValue || value > minValue) throw new InvalidDataException($"The {nameof(ParameterType.U128)} value has to be a 128 bit signed integer but it was: '{value}'.");
        return value.ToByteArray();
    }

    private static byte[] SerializeBool(bool value)
    {
        byte bytes = Convert.ToByte(value);
        return new[] {bytes};
    }

    private static byte[] SerializeString(StringType stringType, string stringValue)
    {
        byte[] stringLengthAsBytes = SerializeLength(stringValue.Length, stringType.SizeLength);
        byte[] stringValueAsBytes = Encoding.UTF8.GetBytes(stringValue);
        return stringLengthAsBytes.Concat(stringValueAsBytes).ToArray();
    }

    private static byte[] SerializeListOrSet(ListType listType, IList listData)
    {
        using MemoryStream buffer = new MemoryStream();
        int length = listData.Count;
        byte[] listLengthAsBytes = SerializeLength(length, listType.SizeLength);
        buffer.Write(listLengthAsBytes);
        foreach (dynamic value in listData)
        {
            byte[] valueAsBytes = Serialize(listType.ValueType, value);
            buffer.Write(valueAsBytes);
        }
        return buffer.ToArray();
    }

    //todo: find out where u32 and u64 is used and think how to inplement it
    private static byte[] SerializeLength(int length, SizeLength sizeLength)
    {
        switch (sizeLength)
        {
            case SizeLength.U8:
                return SerializeUInt8((byte) length);
            case SizeLength.U16:
                return SerializeUInt16((ushort) length, true);
            case SizeLength.U32:
                return SerializeUInt32((uint) length, true);
            case SizeLength.U64:
                return SerializeUInt64((ulong) length, true);
            default:
                throw new IndexOutOfRangeException($"Unknown {nameof(SizeLength)} provided.");
        }
    }

    private static byte[] SerializeArray(ArrayType arrayType, Array arrayData)
    {
        using MemoryStream buffer = new MemoryStream();
        if (arrayType.Size != arrayData.Length) throw new InvalidDataException($"The {nameof(ArrayType)} schema size and array data length not matched.");
        for (uint i = 0; i < arrayType.Size; i++)
        {
            byte[] userValueAsBytes = Serialize(arrayType.ValueType, arrayData.GetValue(i));
            buffer.Write(userValueAsBytes);
        }
        return buffer.ToArray();
    }

    private static byte[] SerializeContractName(StringType stringType, dynamic contractInfo)
    {
        dynamic? contractProperty = contractInfo.GetType().GetProperty("Contract");
        if (contractProperty is null) throw new MissingFieldException($"The {nameof(ParameterType.ContractName)} value misses property 'Contract'.");
        dynamic? contractPropertyValue = contractProperty.GetValue(contractInfo);
        string contractName  = "init_" + contractPropertyValue;
        return SerializeString(stringType, contractName);
    }

    private static byte[] SerializeReceiveName(StringType stringType, dynamic receiveInfo)
    {
        dynamic? contractProperty = receiveInfo.GetType().GetProperty("Contract");
        dynamic? funcProperty = receiveInfo.GetType().GetProperty("Func");
        if (contractProperty is null) throw new MissingFieldException($"The {nameof(ParameterType.ReceiveName)} value misses property 'Contract'.");
        if (funcProperty is null) throw new MissingFieldException($"The {nameof(ParameterType.ReceiveName)} value misses property 'Func'.");
        dynamic? contractPropertyValue = contractProperty.GetValue(receiveInfo);
        dynamic? funcPropertyValue = funcProperty.GetValue(receiveInfo);
        string receiveName = $"{contractPropertyValue}.{funcPropertyValue}";
        return SerializeString(stringType, receiveName);
    }

    private static byte[] SerializePairType(PairType pairType, ArrayList pairData)
    {
        if (pairData.Count != 2) throw new InvalidDataException($"The {nameof(ParameterType.Pair)} value supports only pairs of two.");
        using MemoryStream buffer = new MemoryStream();
        dynamic? leftValue = pairData[0];
        dynamic? rightValue = pairData[1];
        byte[] leftValueAsBytes = Serialize(pairType.LeftType, leftValue);
        byte[] rightValueAsBytes = Serialize(pairType.RightType, rightValue);
        buffer.Write(leftValueAsBytes);
        buffer.Write(rightValueAsBytes);
        return buffer.ToArray();
    }

    //todo: there is no possibility to create a dictionary of size u32 or u64 is it a problem
    private static byte[] SerializeMapType(MapType mapType, IDictionary mapData)
    {
        using MemoryStream buffer = new MemoryStream();
        int length = mapData.Count;
        byte[] mapLengthBuffer = SerializeLength(length, mapType.SizeLength);
        buffer.Write(mapLengthBuffer);
        foreach (dynamic item in mapData)
        {
            byte[] keyAsBytes = Serialize(mapType.KeyType, item.Key);
            byte[] valueAsBytes = Serialize(mapType.ValueType, item.Value);
            buffer.Write(keyAsBytes);
            buffer.Write(valueAsBytes);
        }
        return buffer.ToArray();
    }

    private static byte[] SerializeEnumType(EnumType enumType, object enumData)
    {
        using MemoryStream buffer = new MemoryStream();
        PropertyInfo[] enumDataVariants = enumData.GetType().GetProperties();
        for (int i = 0; i < enumType.Variants.Length; i++)
        {
            PropertyInfo enumDataVariant = enumDataVariants[i];
            object enumDataValue = enumDataVariant.GetValue(enumData)!;
            if (enumDataVariant.Name == enumType.Variants[i].VariantName)
            {
                if (enumType.Variants.Length <= 256)
                {
                    buffer.Write(SerializeUInt8((byte) i));
                }
                else if (enumType.Variants.Length <= 256 * 256)
                {
                    buffer.Write(SerializeUInt16((ushort) i));
                }
                else
                {
                    throw new IndexOutOfRangeException("Enums with more than 65536 variants are not supported.");
                }
                buffer.Write(SerializeSchemaFields(enumType.Variants[i].VariantFields, enumDataValue));
                return buffer.ToArray();
            }
        }
        throw new InvalidDataException("Invalid enum input.");
    }

    private static byte[] SerializeStruct(StructType structType, dynamic structData)
    {
        return SerializeSchemaFields(structType.Fields, structData);
    }

    private static byte[] SerializeSchemaFields(Fields fields, dynamic fieldsData)
    {
        using MemoryStream buffer = new MemoryStream();
        switch (fields.FieldsTag)
        {
            case FieldsTag.Named:
            {
                PropertyInfo[] properties = fieldsData.GetType().GetProperties();
                NamedFields namedFields = fields as NamedFields ?? throw new InvalidCastException($"Invalid named fields type. Expected {nameof(NamedFields)}.");
                if (properties.Length != namedFields.Contents.Length) throw new InvalidDataException($"Expected '{namedFields.Contents.Length}' named fields.");
                foreach (var fieldInfo in namedFields.Contents)
                {
                    dynamic? fieldValue = properties.Single(x => x.Name == fieldInfo.FieldName).GetValue(fieldsData);
                    byte[] userValueAsBytes = Serialize(fieldInfo.FieldType, fieldValue);
                    buffer.Write(userValueAsBytes);
                }
                return buffer.ToArray();
            }
            case FieldsTag.Unnamed:
            {
                UnnamedFields unnamedFields = fields as UnnamedFields ?? throw new InvalidCastException($"Invalid unnamed fields type. Expected {nameof(UnnamedFields)}.");
                if (fieldsData is not ArrayList) throw new InvalidCastException($"The {nameof(UnnamedFields)} value require value of type {nameof(ArrayList)}.");
                if (fieldsData.Count != unnamedFields.Contents.Length) throw new InvalidDataException($"Expected {unnamedFields.Contents.Length} unnamed fields.");
                for (int i = 0; i < unnamedFields.Contents.Length; i++)
                {
                    Type fieldType = unnamedFields.Contents[i];
                    object fieldValue = fieldsData[i];
                    byte[] userValueAsBytes = Serialize(fieldType, fieldValue);
                    buffer.Write(userValueAsBytes);
                }
                return buffer.ToArray();
            }
            case FieldsTag.None:
            default:
            {
                return buffer.ToArray();
            }
        }
    }

    private static ulong GetMilliSeconds(string value)
    {
        ulong milliSeconds = 0;
        uint days = GetDuration(value, new Regex(@"(\d+)\s*d"));
        uint hours = GetDuration(value, new Regex(@"(\d+)\s*h"));
        uint minutes = GetDuration(value, new Regex(@"(\d+)\s*m\b"));
        uint seconds = GetDuration(value, new Regex(@"(\d+)\s*s/g"));
        uint milliseconds = GetDuration(value, new Regex(@"(\d+)\s*ms"));
        milliSeconds += days * 86400 * 1000;
        milliSeconds += hours * 3600 * 1000;
        milliSeconds += minutes * 60 * 1000;
        milliSeconds += seconds * 1000;
        milliSeconds += milliseconds;
        return milliSeconds;
    }

    private static uint GetDuration(string value, Regex regex)
    {
        uint count = 0;
        MatchCollection matches = regex.Matches(value);
        foreach (Match match in matches)
        {
            count += uint.Parse(match.Groups[1].Value);
        }
        return count;
    }
}
