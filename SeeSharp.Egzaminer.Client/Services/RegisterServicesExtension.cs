using SeeSharp.Egzaminer.Client.Services.Auth;

namespace SeeSharp.Egzaminer.Client.Services;

public static class RegisterServicesExtension
{
    public static IServiceCollection AddRegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationProxyService, AuthenticationProxyService>();
        return services;
    }
}
