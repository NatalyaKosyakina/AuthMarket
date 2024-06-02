using AuthMarket.Abstractions;
using AuthMarket.db;
using AuthMarket.Model;
using System.Text;
using System.Security.Cryptography;

namespace AuthMarket.Repo
{
    public class AuthRepo (AuthContext context) : IAuthRepo
    {
        private readonly AuthContext _context = context;

        public void AddUser(string email, string password, UserRoleType userRoleType)
        {
            var checkUser = _context.users.FirstOrDefault(user => user.Email == email);

            if (checkUser == null)
            {

                var newUser = new User() { Email = email, RoleID = userRoleType };
                newUser.Salt = new byte[16];
                new Random().NextBytes(newUser.Salt);
                var data = Encoding.UTF8.GetBytes(password).Concat(newUser.Salt).ToArray();
                newUser.Password = new SHA512Managed().ComputeHash(data);
                _context.Add(newUser);
                _context.SaveChanges();
            }
        }

        public UserRoleType CheckRole(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
