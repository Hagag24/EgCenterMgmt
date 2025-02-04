using Blazored.LocalStorage;
using Blazored.Toast;
using SmartLearnMgmt.Maui.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace SmartLearnMgmt.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

            builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

            builder.Services.AddScoped<CookieHandler>();
            builder.Services.AddScoped<CookieService>();
            string Url = "https://api.egcenter.online";
            //string Url = "https://localhost:7129";
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
            return builder.Build();
        }
    }
}
