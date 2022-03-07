// ---------------------------------------------
//      --- Trellog-Action by Scarementus ---
//      ---        Licence MIT       ---
// ---------------------------------------------

namespace Trellog_Action2;

public class ActionInputs
{
    string _repositoryName = null!;
    string _branchName = null!;

    public ActionInputs()
    {
        if (Environment.GetEnvironmentVariable("GREETINGS") is { Length: > 0 } greetings)
            Console.WriteLine(greetings);
    }

    [Option('k', "apikey", Required = true, HelpText = "Api key for trello")]
    public string ApiKey { get; set; }

    [Option('t', "apitoken", Required = true, HelpText = "Api token for trello")]
    public string ApiToken { get; set; }

    [Option('l', "list", Required = true, HelpText = "Input list to get the data from.")]
    public string InputList { get; set; } = null!;

    [Option('o', "output", Required = false,
        HelpText = "Output list, where to add the card with the release info to, if false will not output to trello")]
    public string OutputList { get; set; } = null!;

    [Option('f', "outputfile", Required = false,
        HelpText = "Output File for the update to be added to, if it doesnt exist will be created")]
    public string OutputFile { get; set; } = null!;

    [Option('v', "version", Required = false, HelpText = "Manual bump of version, if null will bump +1 from previous")]
    public string Version { get; set; } = null!;

    [Option('g', "releaseType", Required = false, Default = "Patch",
        HelpText = "The type of release, Major, Minor or Patch")]
    public string ReleaseType { get; set; }

    [Option('c', "cleanInputList", Default = false,
        HelpText = "Whether or not to delete all the cards from the input list")]
    public bool CleanInputList { get; set; }
}