using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SeeSharp.Egzaminer.Shared.DTO;
using SeeSharp.Egzaminer.Shared.Wrapper;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SeeSharp.Egzaminer.Shared;

namespace SeeSharp.Egzaminer.Client.Services.Auth;

public interface IAuthenticationProxyService
{
    Task<IResult> Register(RegisterRequestDto dto);
    Task<IResult> Login(LoginRequestDto dto);
    Task Logout();
}

public class AuthenticationProxyService : IAuthenticationProxyService
{
    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authProvider;
    private readonly ILocalStorageService localStorage;

    public AuthenticationProxyService(
        IHttpClientFactory httpClientFactory,
        AuthenticationStateProvider authProvider,
        ILocalStorageService localStorage)
    {
        this.httpClient = httpClientFactory.CreateClient(Constants.HttpClientName);
        this.authProvider = authProvider;
        this.localStorage = localStorage;
    }
    public async Task<IResult> Register(RegisterRequestDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/register", dto);
        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        return Result.Fail();
    }

    public async Task<IResult> Login(LoginRequestDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/login", dto);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<LoginResponseDto>>();
            await localStorage.SetItemAsync(Constants.Local_Token, result!.Data.Token);
            await localStorage.SetItemAsync(Constants.Local_UserDetails, result.Data.UserDto);

            ((AuthStateProvider)authProvider).NotifyUserLoggedIn(result.Data.Token!);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Data.Token);
            return result;
        }
        else
        {
            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result!;
        }
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync(Constants.Local_Token);
        await localStorage.RemoveItemAsync(Constants.Local_UserDetails);

        ((AuthStateProvider)authProvider).NotifyUserLogout();

        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
