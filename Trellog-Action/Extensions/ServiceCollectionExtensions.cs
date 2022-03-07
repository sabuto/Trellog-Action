// ---------------------------------------------
//      --- Trellog-Action by Scarementus ---
//      ---        Licence MIT       ---
// ---------------------------------------------


namespace Trellog_Action.Extensions;

static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddGitHubActionServices(this IServiceCollection services) =>
        services.AddSingleton<TrelloService>();
}