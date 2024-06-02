using System.ComponentModel.DataAnnotations;

namespace AuthMarket.Security
{
    public class UserAuthRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public AuthMarket.Model.UserRoleType UserRole { get; set; } = AuthMarket.Model.UserRoleType.User;
    }
}
