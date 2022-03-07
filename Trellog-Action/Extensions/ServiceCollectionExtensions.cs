// ---------------------------------------------
//      --- Trellog-Action by Scarementus ---
//      ---        Licence MIT       ---
// ---------------------------------------------


namespace Trellog_Action2.Extensions;

static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddGitHubActionServices(this IServiceCollection services) =>
        services.AddSingleton<TrelloService>();
}