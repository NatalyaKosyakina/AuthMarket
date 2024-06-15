using AuthMarket.Abstractions;
using AuthMarket.db;
using AuthMarket.Model;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace AuthMarket.Repo
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AuthContext _context;
        private readonly ITokenService _tokenService;

        public AuthRepo(AuthContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public int AddUser(string email, string password)
        {
            bool thisIsAdmin = !_context.Users.Any();
            User? user = null;
            if (!thisIsAdmin)
            {
                user = _context.Users.FirstOrDefault(user => user.Email == email);
            }

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Salt = new byte[16]
                };
                new Random().NextBytes(user.Salt);

                var data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
                SHA512 sha = new SHA512Managed();
                user.Password = sha.ComputeHash(data);
                if (thisIsAdmin)
                {
                    user.RoleID = RoleType.Admin;
                }
                else
                {
                    user.RoleID = RoleType.User;
                }
                _context.Add(user);
                _context.SaveChanges();
            }
            return user.Id;
        }

        public string CheckRole(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email.Equals(email));
            if (user == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                if (CheckPassword(user, password))
                {
                    string roleName = "User";
                    if (user.RoleID.Equals(RoleType.Admin))
                    {
                        roleName = user.RoleID.ToString();
                    }

                    return _tokenService.CreateToken(user.Email, roleName);
                }
                else
                {
                    throw new Exception("Wrong password");
                }
            }
        }

        public static bool CheckPassword(User user, string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password).Concat(user.Salt).ToArray();
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(data);
            return hash.SequenceEqual(user.Password);
        }
    }
}
