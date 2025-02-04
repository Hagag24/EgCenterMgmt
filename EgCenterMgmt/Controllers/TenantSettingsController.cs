namespace EgCenterMgmt.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class TenantSettingsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private static TenantSettings? _tenantSettings;

        public TenantSettingsController(IWebHostEnvironment env)
        {
            _env = env;
            if (_tenantSettings == null)
            {
                _tenantSettings = LoadTenantSettings();
            }
        }

        // الحصول على جميع المستأجرين
        [HttpGet]
        public IActionResult GetTenants()
        {
            return Ok(_tenantSettings!.Tenants);
        }
        [HttpGet]
        public IActionResult GetTenant(string tId)
        {
            var tenant = _tenantSettings!.Tenants.FirstOrDefault(t=>t.TId ==  tId);
            if (tenant == null)
                return NotFound(tId + "غير موجود");
            return Ok(tenant);
        }

        [HttpPost]
        public IActionResult AddTenant([FromBody] Tenant newTenant)
        {
            if (_tenantSettings!.Tenants.Any(t => t.TId == newTenant.TId))
                return BadRequest("Tenant with the same TId already exists.");

            _tenantSettings.Tenants.Add(newTenant);
            SaveTenantSettings();
            return CreatedAtAction(nameof(GetTenants), new { tId = newTenant.TId }, newTenant);
        }

        // تحديث مستأجر موجود
        [HttpPut]
        public IActionResult UpdateTenant(string tId, [FromBody] Tenant updatedTenant)
        {
            var tenant = _tenantSettings!.Tenants.FirstOrDefault(t => t.TId == tId);
            if (tenant == null) return NotFound();

            tenant.Name = updatedTenant.Name;
            tenant.IsActive = updatedTenant.IsActive;
            tenant.StartDate = updatedTenant.StartDate;
            tenant.EndDate = updatedTenant.EndDate;
            tenant.ConnectionString = updatedTenant.ConnectionString;

            SaveTenantSettings();
            return NoContent();
        }

        // حذف مستأجر
        [HttpDelete]
        public IActionResult DeleteTenant(string tId)
        {
            var tenant = _tenantSettings!.Tenants.FirstOrDefault(t => t.TId == tId);
            if (tenant == null) return NotFound();

            _tenantSettings.Tenants.Remove(tenant);
            SaveTenantSettings();
            return NoContent();
        }

        // تحميل إعدادات المستأجر من الملف
        private TenantSettings LoadTenantSettings()
        {
            var tenantSettingsPath = Path.Combine(_env.ContentRootPath, "tenantsettings.json");
            var json = System.IO.File.ReadAllText(tenantSettingsPath);
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            return appSettings!.TenantSettings;
        }

        // حفظ إعدادات المستأجر في الملف
        private void SaveTenantSettings()
        {
            var tenantSettingsPath = Path.Combine(_env.ContentRootPath, "tenantsettings.json");
            var tenantSettingsJson = JsonConvert.SerializeObject(new AppSettings { TenantSettings = _tenantSettings! }, Formatting.Indented);
            System.IO.File.WriteAllText(tenantSettingsPath, tenantSettingsJson);
        }
    }
}
