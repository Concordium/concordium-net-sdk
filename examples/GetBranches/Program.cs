using CommandLine;
using Concordium.Sdk.Client;
using Branch = Concordium.Sdk.Types.Branch;

// We disable these warnings since CommandLine needs to set properties in options
// but we don't want to give default values.
#pragma warning disable CS8618

namespace GetBranches;

internal sealed class GetBranchesOptions
{
    [Option(HelpText = "URL representing the endpoint where the gRPC V2 API is served.",
        Default = "http://grpc.testnet.concordium.com:20000/")]
    public string Endpoint { get; set; }
}


public static class Program
{
    /// <summary>
    /// Example how to use <see cref="ConcordiumClient.GetBranches"/>
    /// </summary>s
    public static async Task Main(string[] args) =>
        await Parser.Default
            .ParseArguments<GetBranchesOptions>(args)
            .WithParsedAsync(Run);

    private static async Task Run(GetBranchesOptions options)
    {
        using var client = new ConcordiumClient(new Uri(options.Endpoint), new ConcordiumClientOptions());

        var branch = await client.GetBranchesAsync();

        PrintBranchesAsTree(0, branch);
    }

    private static void PrintBranchesAsTree(uint depth, Branch branch)
    {
        for (var i = 0; i < depth; i++)
        {
            Console.Write("--");
        }
        Console.WriteLine(branch.BlockHash);
        branch.Children.ForEach(x => PrintBranchesAsTree(depth + 1, x));
    }
}
