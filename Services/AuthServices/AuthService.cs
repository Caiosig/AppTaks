using Domain.Abstractions;
using Domain.Enum;
using Infra.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.AuthService
{
    /// <summary>
    /// Serviço responsável pela geração de tokens JWT para autenticação de usuários.
    /// Utiliza configurações definidas no appsettings (Issuer, Audience, Key, Expiration).
    /// </summary>
    public class AuthService(IConfiguration configuration, TasksDbContext context) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly TasksDbContext _context = context;

        /// <summary>
        /// Gera um token JWT contendo informações do usuário.
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        /// <param name="username">Nome de usuário.</param>
        /// <returns>Token JWT como string.</returns>
        public string GenerateJWT(string email, string username)
        {
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var key = _configuration["JWT:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new ("Email", email),
                new ("Username", username),
                new ("EmailIdentifier", email.Split("@").ToString()!),
                new ("CurrentTime", DateTime.Now.ToString())
            };

            _ = int.TryParse(_configuration["JWT:TokenExpirationTimeInDays"], out int tokenExpirationTimeInDays);

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.Now.AddDays(tokenExpirationTimeInDays), signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gera um token de refresh seguro e aleatório para renovação de autenticação.
        /// Utiliza um gerador criptograficamente seguro para criar um token em Base64.
        /// </summary>
        /// <returns>Token de refresh como string Base64.</returns>
        public string GenerateRefreshJWT()
        {
            // Cria um array de bytes para armazenar o valor aleatório do token de refresh.
            var secureRandomBytews = new byte[128];

            // Instancia um gerador de números aleatórios criptograficamente seguro.
            using var randomNumberGenerator = RandomNumberGenerator.Create();

            // Preenche o array com bytes aleatórios gerados de forma segura.
            randomNumberGenerator.GetBytes(secureRandomBytews);

            // Converte o array de bytes em uma string Base64 para ser utilizada como token de refresh.
            return Convert.ToBase64String(secureRandomBytews);
        }

        /// <summary>
        /// Realiza o hash da senha informada utilizando o algoritmo SHA256.
        /// Transforma a senha em uma string hexadecimal segura para armazenamento.
        /// </summary>
        /// <param name="password">Senha em texto puro a ser hasheada.</param>
        /// <returns>Senha hasheada em formato hexadecimal.</returns>
        public string HashingPassword(string password)
        {
            // Converte a senha em bytes e aplica o hash SHA256.
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();

            // Converte cada byte do hash em representação hexadecimal.
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2"));
            }

            // Retorna a senha hasheada como string hexadecimal.
            return builder.ToString();
        }

        public ValidationFielUserEnum UniqueEmailAbdUserName(string email, string username)
        {
            // Obtém todos os usuários cadastrados no banco de dados.
            var users = _context.Users.ToList();
            // Verifica se o e-mail já está cadastrado no banco de dados.
            var emailExists = users.Exists(u => u.Email == email);
            // Verifica se o nome de usuário já está cadastrado no banco de dados.
            var usernameExists = users.Exists(u => u.UserName == username);

            // Se ambos existem, retorna enum indicando ambos indisponíveis.
            if (emailExists && usernameExists) 
            {
                return ValidationFielUserEnum.UsernameAndEmailUnavailable;
            }
            else if (emailExists) // Se o e-mail já existe, retorna enum indicando e-mail indisponível.
            {
                return ValidationFielUserEnum.EmailUnavailable;
            }
            else if (usernameExists) // Se o nome de usuário já existe, retorna enum indicando username indisponível.
            {
                return ValidationFielUserEnum.UsernameUnavailable;
            }

            // Se ambos estão disponíveis, retorna enum indicando ambos disponíveis.
            return ValidationFielUserEnum.UsernameAndEmailAvailable;
        }
    }
}
