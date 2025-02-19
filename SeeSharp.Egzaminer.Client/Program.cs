using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SeeSharp.Egzaminer.Client.Services;
using SeeSharp.Egzaminer.Client.Services.Auth;
using SeeSharp.Egzaminer.Shared;

namespace SeeSharp.Egzaminer.Client;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}/auth/login")
        });

        builder.Services.AddHttpClient(Constants.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        });
        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient(Constants.HttpClientName));

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        builder.Services.AddScoped<IAuthenticationProxyService, AuthenticationProxyService>();
        builder.Services.AddMudServices().AddBlazoredLocalStorage();

        //publikacja testow
        builder.Services.AddScoped<ITestProxyService, TestProxyService>();

        await builder.Build().RunAsync();
    }
}
