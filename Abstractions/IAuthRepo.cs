using AuthMarket.Model;

namespace AuthMarket.Abstractions
{
    public interface IAuthRepo
    {
        public void AddUser(string username, string password, RoleType userRole);
        public RoleType CheckRole(string email, string password);
    }
}
