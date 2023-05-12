using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;

namespace Concordium.Sdk.Tests.UnitTests;

/// <summary>
/// <see cref="DataAttribute"/> used to specify data captured from resources embedded
/// in the manifest. This is used for the theory in <see cref="Xunit"/> tests.
/// </summary>
public class EmbeddedResourceDataAttribute : DataAttribute
{
    public string[] Args { get; init; }

    public EmbeddedResourceDataAttribute(params string[] args) => this.Args = args;

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var result = new object[this.Args.Length];
        for (var index = 0; index < this.Args.Length; index++)
        {
            result[index] = ReadManifestData(this.Args[index]);
        }
        return new[] { result };
    }

    protected static string ReadManifestData(string resourceName)
    {
        var assembly = typeof(EmbeddedResourceDataAttribute).GetTypeInfo().Assembly;
        resourceName = resourceName.Replace("/", ".");
        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
