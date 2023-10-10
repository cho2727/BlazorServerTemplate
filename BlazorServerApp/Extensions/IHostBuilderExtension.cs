using Serilog;

namespace BlazorServerApp.Extensions;

public static class IHostBuilderExtension
{
    public static void AddSerilog(this IHostBuilder self)
    {
        self.UseSerilog(
        (content, configuration) => configuration
                                    .ReadFrom
                                    .Configuration(content.Configuration));
    }
}
