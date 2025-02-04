
namespace EgCenterMgmt.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private readonly ILogger<TenantService> _logger;
        private HttpContext? _httpContext;
        private Tenant? _currentTenant;
        private string? _currentUserId;

        public TenantService(IHttpContextAccessor contextAccessor, ILogger<TenantService> logger)
        {
            var json = File.ReadAllText("tenantsettings.json");
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            _httpContext = contextAccessor.HttpContext;
            _tenantSettings = appSettings!.TenantSettings;
            _logger = logger;

            if (_httpContext is not null)
            {
                var tenantid = _httpContext.Request.Headers?.FirstOrDefault(t => t.Key == "tenantid").Value.ToString();
                if (!string.IsNullOrEmpty(tenantid))
                {
                    tenantid = CryptoService.DecryptData(tenantid);
                }
                var userid = _httpContext.Request.Headers?.FirstOrDefault(t => t.Key == "userid").Value.ToString();
                if (!string.IsNullOrEmpty(userid))
                {
                    _currentUserId = CryptoService.DecryptData(userid);
                }
                if (!string.IsNullOrEmpty(tenantid))
                {
                    SetCurrentTenant(tenantid);
                }
                else
                {
                    _logger.LogError("No tenant provided in the request headers.");

                }
            }
            else
            {
                _logger.LogWarning("HttpContext is null.");
            }
        }

        public string? GetConnectionString()
        {
            var currentConnectionString = _currentTenant is null
                ? _tenantSettings.Defaults.ConnectionString
                : _currentTenant.ConnectionString;

            return currentConnectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }
        public string? GetCurrentUserId()
        {
            return _currentUserId;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }

        private void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TId == tenantId);

            if (_currentTenant is null)
            {
                throw new Exception("معرف الـ Tenant غير صحيح");
            }

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}
