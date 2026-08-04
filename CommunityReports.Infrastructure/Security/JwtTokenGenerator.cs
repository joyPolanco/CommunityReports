using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommunityReports.Application.DTOs.Identity;
using CommunityReports.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CommunityReports.Infrastructure.Security
{
    /// <summary>
    /// Genera tokens JWT a partir de <see cref="TokenClaimsData"/> (id, nombre,
    /// correo y roles ya resueltos por Identity vía IIdentityService). No depende
    /// de ninguna entidad de dominio ni de Identity directamente.
    /// </summary>
    public sealed class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _settings;

        public JwtTokenGenerator(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public (string Token, DateTime ExpiraEn) GenerarToken(TokenClaimsData claims)
        {
            var expiraEn = DateTime.UtcNow.AddMinutes(_settings.ExpiraEnMinutos);

            var tokenClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, claims.UsuarioId.ToString()),
                new(ClaimTypes.NameIdentifier, claims.UsuarioId.ToString()),
                new(ClaimTypes.Name, claims.NombreUsuario),
                new(ClaimTypes.Email, claims.Correo),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            tokenClaims.AddRange(claims.Roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: tokenClaims,
                expires: expiraEn,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiraEn);
        }
    }
}
