
namespace EgCenterMgmt.Services
{
    public interface IUserManagementServices
    {
        bool IsValidEmail(string email);
    }

    internal class UserManagementServices : IUserManagementServices
    {
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
