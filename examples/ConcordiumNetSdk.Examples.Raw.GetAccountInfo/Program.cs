using ConcordiumNetSdk.Client;
using Concordium.V2;

namespace ConcordiumNetSdk.Examples.Raw;

/// <summary>
/// Example demonstrating how invoke the "raw" <see cref="ConcordiumClient.RawClient.GetAccountInfo" /> GRPC API call.
///
/// <see cref="ConcordiumClient.RawClient"/> wraps methods of the Concordium Node GRPC API V2 that were generated from
/// the protocol buffer schema by the <see cref="Grpc.Core"/> library. Creating an instance of the generated
/// <see cref="AccountInfoRequest"/> used for the method input is given below.
/// </summary>
class Program
{
    static void GetAccountInfoExample(GetAccountInfoExampleOptions options)
    {
        // Construct the client.
        ConcordiumClient client = new ConcordiumClient(
            new Uri(options.Endpoint), // Endpoint URL.
            options.Port, // Port.
            60, // Use a timeout of 60 seconds.
            true // Use a secure connection.
        );

        BlockHashInput blockHashInput;

        switch (options.BlockHash.ToLower())
        {
            case "best":
                blockHashInput = ConcordiumNetSdk.Types.BlockHash.BestBlockHashInput();
                break;
            case "lastfinal":
                blockHashInput = ConcordiumNetSdk.Types.BlockHash.LastFinalBlockHashInput();
                break;
            default:
                blockHashInput = ConcordiumNetSdk.Types.BlockHash
                    .From(options.BlockHash)
                    .ToBlockHashInput();
                break;
        }

        // Construct the input for the "raw" method.
        AccountInfoRequest request = new AccountInfoRequest
        {
            /// Convert command line parameter to a <see cref="ConcordiumNetSdk.Types.BlockHash"/>
            /// and then to a <see cref="BlockHashInput"/> which is needed for the <see cref="AccountInfoRequest"/>.
            BlockHash = blockHashInput,
            /// Convert command line parameter to a <see cref="ConcordiumNetSdk.Types.AccountAddress"/>
            /// and then to a <see cref="AccountIdentifierInput"/> which is needed for the <see cref="AccountInfoRequest"/>.
            AccountIdentifier = ConcordiumNetSdk.Types.AccountAddress
                .From(options.AccountAddress)
                .ToAccountIdentifierInput()
        };

        // Invoke the "raw" call.
        AccountInfo accountInfo = client.Raw.GetAccountInfo(request);

        // Print account info.
        PrintAccountInfo(accountInfo);
    }

    public static void PrintAccountInfo(AccountInfo accountInfo)
    {
        Console.WriteLine(
            $@"
            Address:          {ConcordiumNetSdk.Types.AccountAddress .From(accountInfo.Address.Value.ToArray()) .ToString()}
            Balance:          {accountInfo.Amount.Value.ToString()} CCD
            Sequence number:  {accountInfo.SequenceNumber.Value.ToString()}
        "
        );
    }

    static void Main(string[] args)
    {
        Example.RunExample<GetAccountInfoExampleOptions>(args, GetAccountInfoExample);
    }
}
