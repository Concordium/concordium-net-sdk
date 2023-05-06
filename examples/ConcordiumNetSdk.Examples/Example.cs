using CommandLine;

namespace ConcordiumNetSdk.Examples;

/// <summary>
/// Represents an example program which supports the command-line
/// parameters defined by a sub-class of <see cref="ExampleOptions"/>.
/// </summary>
public static class Example
{
    /// <summary>
    /// Run an example program specified by a callback and the raw command-
    /// line parameters. The raw command line parameters are parsed according
    /// to the supplied <typeparam name="T"/> and the callback is invoked with
    /// the resulting instance as its argument.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="ExampleOptions"/> instance into which <paramref name="args"/>
    /// will be parsed.
    /// </typeparam>
    /// <param name="args">
    /// The raw command line arguments.
    /// </param>
    /// <param name="exampleCallback">
    /// The callback corresponding to the example program which will be
    /// invoked with the parsed <typeparam name="T"/> instance as its
    /// argument.
    /// </param>
    public static void Run<T>(string[] args, Action<T> exampleCallback)
        where T : ExampleOptions
    {
        /// This is a convenience methos which leverages to other overload
        /// to support callbacks of type <see cref="Action{T}"/>.
        async Task exampleCallbackTask(T options)
        {
            // Await to squelch compiler warning.
            await Task.Run(() => exampleCallback(options));
        }

        Run<T>(args, exampleCallbackTask);
    }

    /// <summary>
    /// Run an asynchronous example program specified by a callback and the
    /// raw command-line parameters. The raw command line parameters are parsed
    /// according to the supplied <typeparam name="T"/> and the callback is invoked
    /// with the resulting instance as its argument.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="ExampleOptions"/> instance into which <paramref name="args"/>
    /// will be parsed.
    /// </typeparam>
    /// <param name="args">
    /// The raw command line arguments.
    /// </param>
    /// <param name="exampleCallback">
    /// The asynchronous callback corresponding to the example program which will be
    /// invoked with the parsed <typeparam name="T"/> instance as its argument.
    /// </param>
    public static void Run<T>(string[] args, Func<T, Task> exampleCallback)
        where T : ExampleOptions
    {
        try
        {
            Task.WaitAll(
                Parser.Default
                    .ParseArguments<T>(args)
                    .WithParsedAsync<T>(options => exampleCallback(options))
            );
        }
        catch (Exception e)
        {
            HandleCallbackException(e);
        }
    }

    private static void HandleCallbackException(Exception e)
    {
        Console.WriteLine($"An error occurred while running the example: {e.Message}");
        Environment.Exit(1);
    }

    private static void HandleParseError(IEnumerable<Error> errors) { }
}
