using Concordium.Sdk.Client;
using Concordium.Sdk.Wallets;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Common;
using Ductus.FluentDocker.Services;
using Grpc.Core;

namespace Concordium.Sdk.Tests.IntegrationTests.Utils.LocalNode;

public sealed class LocalNodeFixture : IDisposable
{
    private readonly ICompositeService _service;
    private const string GRpcPort = "20042";
    private const string ServiceName = "local-test-node";
    internal ConcordiumClient Client { get; }

    public LocalNodeFixture()
    {
        var endpoint = new Uri($"http://127.0.0.1:{GRpcPort}");
        this.Client = new ConcordiumClient(endpoint, new ConcordiumClientOptions());

        var file = Path.Combine(Directory.GetCurrentDirectory(), "Utils/LocalNode/docker-compose.yaml");
        this._service = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(file)
            .RemoveOrphans()
            .Wait(ServiceName, (service, i) =>
            {
                // If tried 20 times, then stop.
                if (i == 20)
                {
                    throw new FluentDockerException("Container couldn't start");
                }

                try
                {
                    // Try query node - if fails with internal error wait and try again.
                    _ = this.Client.Raw.GetNodeInfo();
                }
                catch (RpcException ex)
                {
                    if (ex.Status.StatusCode is StatusCode.Internal or StatusCode.Unavailable)
                    {
                        // Wait 1 second before trying again.
                        return 1_000;
                    }

                    throw;
                }
                return 0;
            })
            .Build()
            .Start();
    }

    public static WalletAccount CreateWalletAccount(int id)
    {
        var path = File.ReadAllText($"./Utils/LocalNode/accounts/stagenet-{id + 1}.json");
        var account = WalletAccount.FromWalletKeyExportFormat(path);
        return account!;
    }

    public void Dispose()
    {
        this.Client.Dispose();
        this._service.Dispose();
    }
}
