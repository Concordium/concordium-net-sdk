using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ConcordiumNetSdk.SchemaSerialization;
using ConcordiumNetSdk.SchemaSerialization.Types;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;
using Type = ConcordiumNetSdk.SchemaSerialization.Types.Type;

namespace ConcordiumNetSdk.UnitTests.SchemaSerialization;

public class ContractParametersSerializerTests
{
    [Fact]
    public void Should_correctly_serialize_contract_parameters_in_right_format()
    {
        // Arrange
        var paramType = GetParamType();
        var paramArgument = GetParamArgument();
        var expectedSerializedContractParameters = GetExpectedSerializedContractParameters();

        // Act
        var serializedContractParameters = ContractParametersSerializer.Serialize(paramType, paramArgument);

        // Assert
        serializedContractParameters.Should().BeEquivalentTo(expectedSerializedContractParameters);
    }

    private Type GetParamType()
    {
        var contents = new[]
        {
            ("Bool", new Type(ParameterType.Bool)),
            ("U8", new Type(ParameterType.U8)),
            ("U16", new Type(ParameterType.U16)),
            ("U32", new Type(ParameterType.U32)),
            ("U64", new Type(ParameterType.U64)),
            ("U128", new Type(ParameterType.U128)),
            ("I8", new Type(ParameterType.I8)),
            ("I16", new Type(ParameterType.I16)),
            ("I32", new Type(ParameterType.I32)),
            ("I64", new Type(ParameterType.I64)),
            ("I128", new Type(ParameterType.I128)),
            ("Amount", new Type(ParameterType.Amount)),
            ("AccountAddress", new Type(ParameterType.AccountAddress)),
            ("ContractAddress", new Type(ParameterType.ContractAddress)),
            ("Timestamp", new Type(ParameterType.Timestamp)),
            ("Duration", new Type(ParameterType.Duration)),
            ("Pair", new PairType(new Type(ParameterType.I32), new Type(ParameterType.Bool))),
            ("Array", new ArrayType(2, new Type(ParameterType.I32))),
            ("Map", new MapType(
                SizeLength.U8,
                new Type(ParameterType.I32),
                new Type(ParameterType.Bool))),
            ("List", new ListType(
                SizeLength.U8,
                new Type(ParameterType.I32),
                ParameterType.List)),
            ("Set", new ListType(
                SizeLength.U8,
                new Type(ParameterType.I32),
                ParameterType.Set)),
            ("Struct", new StructType(
                new NamedFields(
                    new (string FieldName, Type FieldType)[]
                    {
                        ("AccountAddress", new Type(ParameterType.AccountAddress)),
                        ("Amount", new Type(ParameterType.Amount))
                    }))),
            ("Enumerable", new EnumType(
                new (string VariantName, Fields VariantFields)[]
                {
                    ("Some", new UnnamedFields(
                        new[]
                        {
                            new Type(ParameterType.I32),
                            new Type(ParameterType.Bool)
                        })),
                    ("Extra", new NamedFields(
                        new[]
                        {
                            ("One", new Type(ParameterType.I32)),
                            ("Two", new Type(ParameterType.I32))
                        }))
                })),
            ("String", new StringType(SizeLength.U8)),
            ("InitName", new StringType(SizeLength.U8, ParameterType.ContractName)),
            ("ReceiveName", new StringType(SizeLength.U8, ParameterType.ReceiveName))
        };
        var fields = new NamedFields(contents);
        return new StructType(fields);
    }

    private object GetParamArgument()
    {
        return new
        {
            Bool = true,
            U8 = byte.MaxValue,
            U16 = ushort.MaxValue,
            U32 = uint.MaxValue,
            U64 = ulong.MaxValue,
            U128 = BigInteger.Parse("340282366920938463463374607431768211455"),
            I8 = sbyte.MinValue,
            I16 = short.MinValue,
            I32 = int.MinValue,
            I64 = long.MinValue,
            I128 = BigInteger.Parse("-170141183460469231731687303715884105728"),
            Amount = CcdAmount.FromMicroCcd(10),
            AccountAddress = AccountAddress.From("4hvvPeHb9HY4Lur7eUZv4KfL3tYBug8DRc4X9cVU8mpJLa1f2X"),
            ContractAddress = ContractAddress.Create(10, 0),
            Timestamp = new DateTime(1998, 2, 20, 1, 1, 1),
            Duration = "1d 1h 1m 1s 1ms",
            Pair = new ArrayList {1, true},
            Array = new[] {1, 2},
            Map = new Dictionary<int, bool> {{1, true}},
            List = new List<int> {1, 2, 3},
            Set = new List<int> {1, 2, 3},
            Struct = new
            {
                AccountAddress = AccountAddress.From("4hvvPeHb9HY4Lur7eUZv4KfL3tYBug8DRc4X9cVU8mpJLa1f2X"),
                Amount = CcdAmount.FromMicroCcd(10)
            },
            Enumerable = new {Some = new ArrayList{1, true}, Extra = new {One = 1, Two = 2}},
            String = "foo",
            InitName = new {Contract = "contract"},
            ReceiveName = new {Contract = "contract", Func = "func"}
        };
    }

    private byte[] GetExpectedSerializedContractParameters()
    {
        return new byte[]
        {
            1, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 128, 0, 128, 0, 0, 0, 128, 0, 0, 0, 0, 0, 0, 0, 128, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 10, 0, 0, 0, 0, 0, 0, 0, 232, 6, 43, 32, 201, 212, 138, 49, 162,
            145, 233, 196, 62, 107, 120, 24, 85, 116, 103, 166, 131, 226, 23, 195, 157, 203, 242, 27, 111, 91, 122, 90,
            10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 132, 30, 37, 178, 11, 191, 8, 225, 52, 94, 5, 0, 0, 0,
            0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 2, 0, 0, 0, 1, 1, 0, 0, 0, 1, 3, 1, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 3, 1, 0,
            0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 232, 6, 43, 32, 201, 212, 138, 49, 162, 145, 233, 196, 62, 107, 120, 24, 85,
            116, 103, 166, 131, 226, 23, 195, 157, 203, 242, 27, 111, 91, 122, 90, 10, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
            0, 1, 3, 102, 111, 111, 13, 105, 110, 105, 116, 95, 99, 111, 110, 116, 114, 97, 99, 116, 13, 99, 111, 110,
            116, 114, 97, 99, 116, 46, 102, 117, 110, 99
        };
    }
}
