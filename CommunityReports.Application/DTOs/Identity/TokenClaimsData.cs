namespace CommunityReports.Application.DTOs.Identity
{
    /// <summary>Datos necesarios para emitir un JWT. No depende de Usuario ni de Identity.</summary>
    public sealed record TokenClaimsData(
        int UsuarioId,
        string NombreUsuario,
        string Correo,
        IReadOnlyList<string> Roles);
}
