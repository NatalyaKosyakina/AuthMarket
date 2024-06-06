namespace AuthMarket.Model
{
    public class User
    {
        public int Id { get; set; }
        public RoleType RoleID { get; set; }
        public virtual Role Role { get; set; }
        public string Email { get; set; }

        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
