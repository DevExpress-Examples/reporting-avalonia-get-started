using System;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using AvaloniaApplication1;
using DevExpress.DataAccess.Native.Json;

[assembly: SupportedOSPlatform("browser")]

internal sealed partial class Program
{
    private static Task Main(string[] args) {
        JsonLoaderHelper.ConfigureHttpClient = (client, authentication) => {
            return;
        };
        JsonLoaderHelper.ConfigureHttpRequestMessage = (message, authorizationInfo) => {
            if (authorizationInfo != null && !string.IsNullOrWhiteSpace(authorizationInfo.Username)) {
                var authenticationString = $"{authorizationInfo.Username}:{authorizationInfo.Password}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
                message.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            }
        };

        return BuildAvaloniaApp()
            .WithInterFont()
            .UseReactiveUI()
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}
