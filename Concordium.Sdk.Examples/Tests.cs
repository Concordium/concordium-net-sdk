using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using Concordium.Sdk.Client;
using Xunit.Abstractions;

namespace Concordium.Sdk.Examples;

[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
public class Tests : IDisposable
{
    private readonly JsonDocument _json;

    protected ITestOutputHelper Output { get; }
    protected ConcordiumClient Client { get; }

    protected Tests(ITestOutputHelper output)
    {
        this.Output = output;

        var assemblyPath = typeof(Tests).GetTypeInfo().Assembly.Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
        var jsonPath = Path.Combine(assemblyDirectory!, "test_configuration.json");
        this._json = JsonDocument.Parse(File.ReadAllText(jsonPath));

        var endpoint = this.GetString("endpoint");
        var port = this.GetUInt16("port");

        this.Client = new ConcordiumClient(new Uri(endpoint), port);
    }

    protected string GetString(string name) => this.GetConfiguration(name).GetString()!;

    private ushort GetUInt16(string name) => this.GetConfiguration(name).GetUInt16();

    private JsonElement GetConfiguration(string name)
    {
        var jsonElement = this._json.RootElement.GetProperty(name);
        return jsonElement;
    }

    protected static ArgumentOutOfRangeException CreateArgumentOutOfRangeException<T>(T missing) => new($"unknown type: {missing!.GetType()}");

    public void Dispose()
    {
        this._json.Dispose();
        this.Client.Dispose();
    }
}
