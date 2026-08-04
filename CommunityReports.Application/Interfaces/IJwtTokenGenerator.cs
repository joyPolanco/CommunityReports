using CommunityReports.Application.DTOs.Identity;

namespace CommunityReports.Application.Interfaces
{
    /// <summary>Abstracción de generación de tokens JWT (implementada en Infrastructure).</summary>
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpiraEn) GenerarToken(TokenClaimsData claims);
    }
}
