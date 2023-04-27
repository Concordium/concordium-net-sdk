using System;
using System.Linq;
using System.Buffers.Binary;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class DataTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(Data.MaxLength)]
    public void From_OnValidData_ReturnsCorrectValue(Int16 length)
    {
        byte[] header = new Byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(header, (UInt16)length);
        byte[] data = Enumerable.Repeat((byte)128, length).ToArray();
        byte[] bytes = header.Concat(data).ToArray();
        Data.From(data).GetBytes().Should().BeEquivalentTo(bytes);
    }

    [Theory]
    [InlineData(Data.MaxLength + 1)]
    [InlineData(Data.MaxLength + 100)]
    public void From_OnTooLongData_ThrowsException(Int16 length)
    {
        byte[] data = Enumerable.Repeat((byte)128, length).ToArray();
        Action result = () => Data.From(data);
        result.Should().Throw<ArgumentException>();
    }
}
