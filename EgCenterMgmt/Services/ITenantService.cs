
namespace EgCenterMgmt.Services
{
    public interface ITenantService
    {
        string? GetDatabaseProvider();
        string? GetConnectionString();
        Tenant? GetCurrentTenant();
        string? GetCurrentUserId();
    }
}
