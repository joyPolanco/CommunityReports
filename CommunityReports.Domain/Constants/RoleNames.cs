namespace CommunityReports.Domain.Constants
{
    /// <summary>
    /// Nombres canónicos de los roles del sistema. Viven en Domain (no en
    /// Infrastructure) porque son un concepto de negocio -quién puede hacer qué-,
    /// no un detalle de Identity; Identity solo los usa como strings para crear los
    /// roles reales en la base de datos (ver IdentitySeeder). Son <c>const</c> para
    /// poder usarse en atributos <c>[Authorize(Roles = ...)]</c>, que exigen
    /// expresiones constantes en tiempo de compilación.
    /// </summary>
    public static class RoleNames
    {
        /// <summary>Administra el sistema completo: usuarios, catálogos e instituciones.</summary>
        public const string Admin = "Admin";

        /// <summary>Reporta incidencias comunitarias. Tiene perfil de dominio (<see cref="Entities.Ciudadano"/>).</summary>
        public const string Ciudadano = "Ciudadano";

        /// <summary>Gestiona incidencias en nombre de una institución. Tiene perfil de dominio (<see cref="Entities.Empleado"/>).</summary>
        public const string Empleado = "Empleado";

        /// <summary>Todos los roles reconocidos, usado para sembrarlos en la base de datos al iniciar.</summary>
        public static readonly IReadOnlyList<string> Todos = new[] { Admin, Ciudadano, Empleado };
    }
}
