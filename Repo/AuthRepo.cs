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

        public void AddUser(string email, string password, RoleType userRoleType)
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

        public RoleType CheckRole(string email, string password)
        {
            var user = _context.users.FirstOrDefault(user => user.Email.Equals(email));
            if (user == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                var hash = new SHA512Managed().
                    ComputeHash(Encoding.UTF8.GetBytes(password).
                    Concat(user.Salt).ToArray());
                if (hash.Equals(user.Password))
                {
                    return user.RoleID;
                }
                else
                {
                    throw new Exception("Wrong password");
                }
            }
        }
    }
}
