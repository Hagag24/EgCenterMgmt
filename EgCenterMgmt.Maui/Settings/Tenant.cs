namespace SmartLearnMgmt.Shared.Settings;
public class Tenant
{
    public string Name { get; set; } = null!;
    public string TId { get; set; } = null!;
    public string? ConnectionString { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}