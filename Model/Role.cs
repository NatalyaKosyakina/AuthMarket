using System.ComponentModel.DataAnnotations;

namespace AuthMarket.Model
{
    public class Role
    {
        [Key]
        public RoleType RoleID { get; set; }
        public string RoleName { get; set; }
        public virtual List<User> Users { get; set; } = [];
    }
}
