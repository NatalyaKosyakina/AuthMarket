using AuthMarket.Abstractions;
using AuthMarket.db;
using AuthMarket.Model;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace AuthMarket.Repo
{
    public class AuthRepo (AuthContext context) : IAuthRepo
    {
        private readonly AuthContext _context = context;

        public int AddUser(string email, string password)
        {
            bool thisIsAdmin = _context.Users.Count() == 0;
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
                var data = Encoding.UTF8.GetBytes(password).Concat(user.Salt).ToArray();
                user.Password = SHA512.HashData(data);
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

        public RoleType CheckRole(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email.Equals(email));
            if (user == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                var hash = SHA512.HashData(Encoding.UTF8.GetBytes(password).Concat(user.Salt).ToArray());
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
