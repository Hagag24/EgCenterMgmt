using EgCenterMgmt.Client.Identity;
using EgCenterMgmt.Shared.ModelsAuth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace EgCenterMgmt.Client.Identity;

public class CookieAuthenticationStateProvider : AuthenticationStateProvider, IAccountManagement
{
    private bool _authenticated = false;

    private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

    private readonly HttpClient _httpClient;


    private readonly JsonSerializerOptions jsonSerializerOptions =
      new(){ PropertyNamingPolicy = JsonNamingPolicy.CamelCase,};
    private readonly CookieService _cookieService;

    public CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory,CookieService cookieService)
    {
        _httpClient = httpClientFactory.CreateClient("Auth");
        _cookieService = cookieService;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _authenticated = false;

        // default to not authenticated
        var user = Unauthenticated;

        try
        {
            var userResponse = await _httpClient.GetAsync("api/UserManagement/GetUserInfo");

            userResponse.EnsureSuccessStatusCode();
            var content = await userResponse.Content.ReadAsStringAsync();

            var userJson = await userResponse.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);

            if (userInfo != null)
            {

        
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInfo.Name),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.UserId.ToString())
                };

                claims.AddRange(
                  userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                      .Select(c => new Claim(c.Key, c.Value)));

                var roles = userInfo.Roles;
                if (roles != null && roles?.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new(ClaimTypes.Role, role));
                    }
                }

                var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                user = new ClaimsPrincipal(id);
                _authenticated = true;
                if (userInfo.TenantId is not null)
                {
                    await SetCookie("TenantId", userInfo.TenantId);
                }
                if (userInfo.TenantId is not null)
                {
                    await SetCookie("UserId", userInfo.UserId.ToString());
                }
                UserDaitels.Roles = userInfo.Roles;
                UserDaitels.UserId = userInfo.UserId;
                UserDaitels.TenantName = userInfo.TenantName;

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        return new AuthenticationState(user);

    }
    private async Task SetCookie(string Key, string Data)
    {
        await _cookieService.RemoveAsync(Key);
        await _cookieService.SetAsync(Key, Data);
    }
    public async Task<AuthResult> LoginAsync(LoginModel credentials)
    {
        try
        {
            if (credentials.TenantId is not null)
            {
                await SetCookie("TenantId",credentials.TenantId);
            }
            var response = await _httpClient.PostAsJsonAsync("api/UserManagement/login?useCookies=true", new
            {
                credentials.Email,
                credentials.Password,
            });

            if (response.IsSuccessStatusCode)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return new AuthResult { Succeeded = true };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login failed: {errorContent}");
                return new AuthResult
                {
                    Succeeded = false,
                    ErrorList = new[] { "Invalid email or password" }
                };
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HttpRequestException: {ex.Message}");
            return new AuthResult
            {
                Succeeded = false,
                ErrorList = new[] { "Network error occurred. Please try again later." }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return new AuthResult
            {
                Succeeded = false,
                ErrorList = new[] { "An unexpected error occurred. Please try again later." }
            };
        }
    }
    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        //await _httpClient.PostAsync("api/UserManagement/Logout?emptyContent);
        var response = await _httpClient.PostAsync("api/UserManagement/Logout?empty={0}", emptyContent);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}