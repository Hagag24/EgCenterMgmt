namespace EgCenterMgmt.Shared.Contracts;

public interface IMustHaveTenant
{
    public string TenantId { get; set; }
}
public interface IMustHaveUser
{
    public Guid UserId { get; set; }
}