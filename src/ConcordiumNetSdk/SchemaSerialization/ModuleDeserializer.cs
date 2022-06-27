using System.Buffers.Binary;
using System.Text;
using ConcordiumNetSdk.SchemaSerialization.Types;
using NBitcoin;
using Type = ConcordiumNetSdk.SchemaSerialization.Types.Type;

namespace ConcordiumNetSdk.SchemaSerialization;

//todo: cover it with unit test
/// <summary>
/// Represents a module deserializer.
/// </summary>
public static class ModuleDeserializer
{
    /// <summary>
    /// Deserialize module.
    /// </summary>
    /// <param name="moduleAsBytes">the module as bytes.</param>
    /// <returns><see cref="Module"/> - module.</returns>
    public static Module Deserialize(byte[] moduleAsBytes)
    {
        MemoryStream stream = new MemoryStream(moduleAsBytes);
        Dictionary<string, Contract> contractSchemas = DeserializeDictionary(DeserializeString, DeserializeContract, stream);
        return new Module(contractSchemas);
    }

    private static Dictionary<TKey, TValue> DeserializeDictionary<TKey, TValue>(
        Func<MemoryStream, TKey> deserializeKey,
        Func<MemoryStream, TValue> deserializeValue,
        MemoryStream stream) where TKey : notnull
    {
        uint length = DeserializeUint32(stream);
        Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>((int) length);
        for (uint i = 0; i < length; i++)
        {
            TKey key = deserializeKey(stream);
            TValue value = deserializeValue(stream);
            dictionary.Add(key, value);
        }
        return dictionary;
    }

    private static uint DeserializeUint32(MemoryStream stream)
    {
        Span<byte> buffer = stream.ReadBytes(4);
        return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }

    private static string DeserializeString(MemoryStream stream)
    {
        byte[] bytes = DeserializeArray(DeserializeUint8, stream);
        return Encoding.UTF8.GetString(bytes);
    }

    private static T[] DeserializeArray<T>(Func<MemoryStream, T> deserialize, MemoryStream stream)
    {
        uint length = DeserializeUint32(stream);
        T[] array = new T[length];
        for (uint i = 0; i < length; i++)
        {
            array[i] = deserialize(stream);
        }
        return array;
    }

    private static byte DeserializeUint8(MemoryStream stream)
    {
        return stream.ReadBytes(1).Single();
    }

    private static Contract DeserializeContract(MemoryStream stream)
    {
        Type? state = DeserializeOption(DeserializeType, stream);
        Type? init = DeserializeOption(DeserializeType, stream);
        Dictionary<string, Type> receive = DeserializeDictionary(DeserializeString, DeserializeType, stream);
        return new Contract(state, init, receive);
    }

    private static T? DeserializeOption<T>(Func<MemoryStream, T?> deserialize, MemoryStream stream) where T : class
    {
        byte tag = DeserializeUint8(stream);
        switch (tag)
        {
            case (byte) OptionTag.None:
                return null;
            case (byte) OptionTag.Some:
                return deserialize(stream);
            default:
                throw new IndexOutOfRangeException($"Unsupported option tag: '{tag}'.");
        }
    }

    private static Type DeserializeType(MemoryStream stream)
    {
        byte tag = DeserializeUint8(stream);
        ParameterType typeTag = (ParameterType) tag;
        switch (typeTag)
        {
            case ParameterType.Unit:
            case ParameterType.Bool:
            case ParameterType.U8:
            case ParameterType.U16:
            case ParameterType.U32:
            case ParameterType.U64:
            case ParameterType.U128:
            case ParameterType.I8:
            case ParameterType.I16:
            case ParameterType.I32:
            case ParameterType.I64:
            case ParameterType.I128:
            case ParameterType.Amount:
            case ParameterType.AccountAddress:
            case ParameterType.ContractAddress:
            case ParameterType.Timestamp:
            case ParameterType.Duration:
            {
                return new Type(typeTag);
            }
            case ParameterType.Pair:
            {
                Type left = DeserializeType(stream);
                Type rightType = DeserializeType(stream);
                return new PairType(left, rightType);
            }
            case ParameterType.List:
            case ParameterType.Set:
            {
                SizeLength sizeLength = (SizeLength) DeserializeUint8(stream);
                Type valueType = DeserializeType(stream);
                return new ListType(sizeLength, valueType, typeTag);
            }
            case ParameterType.Map:
            {
                SizeLength sizeLength = (SizeLength) DeserializeUint8(stream);
                Type keyType = DeserializeType(stream);
                Type valueType = DeserializeType(stream);
                return new MapType(sizeLength, keyType, valueType);
            }            
            case ParameterType.Array:
            {
                uint size = DeserializeUint32(stream);
                Type valueType = DeserializeType(stream);
                return new ArrayType(size, valueType);
            }            
            case ParameterType.Struct:
            {
                Fields fields = DeserializeFields(stream);
                return new StructType(fields);
            }
            case ParameterType.Enum:
            {
                (string, Fields)[] variants = DeserializeArray(x => DeserializeTuple(DeserializeString, DeserializeFields, x), stream);
                return new EnumType(variants);
            }            
            case ParameterType.String:
            case ParameterType.ContractName:
            case ParameterType.ReceiveName:
            {
                SizeLength sizeLength = (SizeLength) DeserializeUint8(stream);
                return new StringType(sizeLength, typeTag);
            }
            default:
            {
                throw new IndexOutOfRangeException($"Unsupported type tag: '{tag}'.");
            }
        }
    }

    private static Fields DeserializeFields(MemoryStream stream)
    {
        byte tag = DeserializeUint8(stream);
        FieldsTag fieldsTag = (FieldsTag) tag;
        switch (fieldsTag)
        {
            case FieldsTag.Named:
            {
                (string, Type)[] contents = DeserializeArray(x => DeserializeTuple(DeserializeString, DeserializeType, x), stream);
                return new NamedFields(contents);
            }
            case FieldsTag.Unnamed:
            {
                Type[] contents = DeserializeArray(DeserializeType, stream);
                return new UnnamedFields(contents);
            }
            case FieldsTag.None:
            {
                return new Fields(fieldsTag);
            }
            default:
            {
                throw new IndexOutOfRangeException($"Unsupported fields tag: '{tag}'.");
            }
        }
    }

    private static (L, R) DeserializeTuple<L, R>(
        Func<MemoryStream, L> deserializeLeft,
        Func<MemoryStream, R> deserializeRight,
        MemoryStream stream)
    {
        L left = deserializeLeft(stream);
        R right = deserializeRight(stream);
        return (left, right);
    }
}
