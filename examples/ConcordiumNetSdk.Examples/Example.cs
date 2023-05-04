using CommandLine;

namespace ConcordiumNetSdk.Examples;

public class Example
{
    public static void RunExample<T>(string[] args, Action<T> exampleCallback)
        where T : ExampleOptions
    {
        try
        {
            Parser.Default
                .ParseArguments<T>(args)
                .WithParsed<T>(options => exampleCallback(options))
                .WithNotParsed(errors => HandleParseError(errors));
        }
        catch (Exception e)
        {
            HandleCallbackException(e);
        }
    }

    public static void HandleCallbackException(Exception e)
    {
        Console.WriteLine($"An error occurred while running the example: {e.Message}.");
        Environment.Exit(1);
    }

    public static void HandleParseError(IEnumerable<Error> errors)
    {
        Console.WriteLine("The following error(s) occurred while parsing command line arguments:");
        foreach (Error error in errors)
        {
            Console.WriteLine(error.ToString());
        }
        Environment.Exit(1);
    }
}
