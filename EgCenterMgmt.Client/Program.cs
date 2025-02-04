using Blazored.LocalStorage;
using Blazored.Toast;
using EgCenterMgmt.Client;
using EgCenterMgmt.Client.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddScoped<CookieHandler>();
builder.Services.AddScoped<CookieService>();
string Url = builder.Configuration["BackendUrl"] ?? string.Empty;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Url) });

builder.Services.AddHttpClient("Auth", options =>
{
    options.BaseAddress = new Uri(Url);
})
.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Auth"));

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddBlazorBootstrap();
await builder.Build().RunAsync();