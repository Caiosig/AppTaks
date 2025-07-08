using Domain.Enum;

namespace Domain.Abstractions
{
    public interface IAuthService
    {
        public string GenerateJWT(string email, string username);

        public string GenerateRefreshJWT();

        public string HashingPassword(string password);

        public ValidationFielUserEnum UniqueEmailAbdUserName(string email, string username);
    }
}
