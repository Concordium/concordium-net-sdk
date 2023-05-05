using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;

namespace ConcordiumNetSdk.UnitTests;

/// <summary>
/// <see cref="DataAttribute"/> used to specify data captured from resources embedded
/// in the manifest. This is used for the theory in <see cref="Xunit"/> tests.
/// </summary>
public class EmbeddedResourceDataAttribute : DataAttribute
{
    public readonly string[] _args;

    public EmbeddedResourceDataAttribute(params string[] args)
    {
        _args = args;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var result = new object[_args.Length];
        for (var index = 0; index < _args.Length; index++)
        {
            result[index] = ReadManifestData(_args[index]);
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
