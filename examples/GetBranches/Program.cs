using CommandLine;
using Concordium.Sdk.Client;
using Branch = Concordium.Sdk.Types.Branch;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBranches;

internal sealed class GetNodeInfoOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://node.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBranches"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetNodeInfoOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetNodeInfoOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var branch = await client.GetBranchesAsync();

        // Prints branches as a tree
        printer(0, branch);
    }

    private static void printer(uint depth, Branch branch) {
        for (int i = 0; i < depth; i++) {
            Console.Write("--");
        }
        Console.WriteLine(branch.BlockHash);
        branch.Children.ForEach(x => printer(depth+1, x));
    }
}
