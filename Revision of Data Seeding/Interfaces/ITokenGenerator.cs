namespace Revision_of_Data_Seeding.Interfaces
{
    public interface ITokenGenerator
    {
        public string GenerateToken(Guid id, string username, string role);
        public string GenerateRefreshToken();
        public string HashToken(string token);
    }
}
