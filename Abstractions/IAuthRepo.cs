using AuthMarket.Model;

namespace AuthMarket.Abstractions
{
    public interface IAuthRepo
    {
        public int AddUser(string username, string password);
        public RoleType CheckRole(string email, string password);
    }
}
