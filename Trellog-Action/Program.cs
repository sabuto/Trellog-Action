using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services.AddGitHubActionServices())
    .Build();

static TService Get<TService>(IHost host) where TService : notnull => host.Services.GetRequiredService<TService>();


var parser = Default.ParseArguments<ActionInputs>(() => new ActionInputs(), args);
parser.WithNotParsed(e =>
{
    Get<ILoggerFactory>(host).CreateLogger("Sabuto.TrellogAction")
        .LogError(message: $"{Environment.NewLine}{e.Select(e => e.ToString())}");
    Environment.Exit(2);
});

static async Task StartTrellog(ActionInputs inputs, IHost host)
{
    if (inputs.ReleaseType is not "Major" or "Minor" or "Patch")
        inputs.ReleaseType = "Patch";
    TrelloService service = Get<TrelloService>(host);

    service.Initialise(inputs.ApiKey, inputs.ApiToken);

    await service.ProcessRelease(inputs);

    var markdown = service.BuildMarkdown();

    string path = Path.Combine(Directory.GetCurrentDirectory(), "document.md");
    await using (StreamWriter w = File.AppendText(path))
    {
        w.Write(markdown);
    }

    await service.UpdateTrello(inputs);

    Console.WriteLine($"::set-output name=changelog::{markdown}");
    Console.WriteLine($"::set-output name=version::{service.VersionString}");
    Console.WriteLine($"::set-output name=versionNumber::{service.VersionNum}");

    Environment.Exit(0);
}

await parser.WithParsedAsync(options => StartTrellog(options, host));
await host.RunAsync();