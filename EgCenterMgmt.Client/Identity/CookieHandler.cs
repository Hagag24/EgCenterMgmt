using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace EgCenterMgmt.Client.Identity
{
    public class CookieHandler : DelegatingHandler
    {
        CookieService _CookieService;
        public CookieHandler(CookieService cookieService)
        {
            _CookieService = cookieService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var TenantId = await _CookieService.GetAsync("TenantId");
            var UserId = await _CookieService.GetAsync("UserId");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("tenantid", TenantId);
            request.Headers.Add("userid", UserId);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
