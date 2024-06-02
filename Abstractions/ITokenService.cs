namespace AuthMarket.Abstractions
{
    public interface ITokenService
    {
        public string CreateToken(string email, string roleName);
    }
}
