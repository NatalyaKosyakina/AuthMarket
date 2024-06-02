using AuthMarket.Model;

namespace AuthMarket.Abstractions
{
    public interface IAuthRepo
    {
        public void AddUser(string username, string password, UserRoleType userRole);
        public UserRoleType CheckRole(string email, string password);
    }
}
