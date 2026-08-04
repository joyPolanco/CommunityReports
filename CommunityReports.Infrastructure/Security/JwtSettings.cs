namespace CommunityReports.Infrastructure.Security
{
    /// <summary>Opciones de JWT enlazadas desde la sección "Jwt" de appsettings.json.</summary>
    public sealed class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiraEnMinutos { get; set; } = 120;
    }
}
