using Common;
using Concordium.Grpc.V2;
using Concordium.Sdk.Client;

namespace RawClient.GetAccountInfo;

/// <summary>
/// Example demonstrating the use of <see cref="Client.RawClient.GetAccountInfo"/>.
///
/// <see cref="RawClient"/> wraps methods of the Concordium Node gRPC API V2 that were generated
/// from the protocol buffer schema by the <see cref="Grpc.Core"/> library. Creating an instance
/// of the generated <see cref="AccountInfoRequest"/> class used for the method input is given below.
/// </summary>
internal class Program
{
    private static void GetAccountInfo(GetAccountInfoExampleOptions options)
    {
        // Construct the client.
        var clientOptions = new ConcordiumClientOptions
        {
            Timeout = TimeSpan.FromSeconds(options.Timeout)
        };
        using var client = new ConcordiumClient(new Uri($"{options!.Endpoint}:{options.Port}"), clientOptions);

        var blockHashInput = options.BlockHash.ToLowerInvariant() switch
        {
            "best" => new BlockHashInput() { Best = new Empty() },
            "lastfinal" => new BlockHashInput() { LastFinal = new Empty() },
            _ => Concordium.Sdk.Types.BlockHash.From(options.BlockHash).ToBlockHashInput(),
        };

        // Construct the input for the raw method.
        var request = new AccountInfoRequest
        {
            /// Convert command line parameter to a <see cref="Types.BlockHash"/>
            /// and then to a <see cref="BlockHashInput"/> which is needed for the <see cref="AccountInfoRequest"/>.
            BlockHash = blockHashInput,
            /// Convert command line parameter to a <see cref="Types.AccountAddress"/>
            /// and then to a <see cref="AccountIdentifierInput"/> which is needed for the <see cref="AccountInfoRequest"/>.
            AccountIdentifier = Concordium.Sdk.Types.AccountAddress
                .From(options.AccountAddress)
                .ToAccountIdentifierInput()
        };

        // Invoke the raw call.
        var accountInfo = client.Raw.GetAccountInfo(request);

        // Print account info.
        PrintAccountInfo(accountInfo);
    }

    private static void PrintAccountInfo(AccountInfo accountInfo) =>
        Console.WriteLine(
            $@"
            Address:          {Concordium.Sdk.Types.AccountAddress.From(accountInfo.Address.Value.ToArray())}
            Balance:          {accountInfo.Amount.Value} CCD
            Sequence number:  {accountInfo.SequenceNumber.Value}
        "
        );

    private static void Main(string[] args) =>
        Example.Run<GetAccountInfoExampleOptions>(args, GetAccountInfo);
}
