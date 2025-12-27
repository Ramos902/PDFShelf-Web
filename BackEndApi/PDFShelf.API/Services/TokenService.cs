using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PDFShelf.Api.Models;

namespace PDFShelf.Api.Services
{
    public class TokenService
    {
        private readonly string _privateKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration config)
        {
            _privateKey = config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            _issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
            _audience = config["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
        }

        public string Generate(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_privateKey);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
                Issuer = _issuer,
                Audience = _audience
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        public Guid GetUserIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Token não fornecido.");

            JwtSecurityToken jwt;

            try
            {
                // Nota: O ReadJwtToken NÃO valida o token, só lê.
                // A validação real é feita pelo middleware [Authorize]
                jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao ler o token JWT. Erro: {ex.Message}");
            }

            var claim = jwt.Claims.FirstOrDefault(c => c.Type == "nameid");

            if (claim == null)
                throw new Exception("Claim 'nameid' não encontrada no token.");

            if (!Guid.TryParse(claim.Value, out Guid userId))
                throw new Exception("O valor da claim 'nameid' não é um GUID válido.");

            return userId;
        }

        public string? GetUserRoleFromToken(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }
    }
}