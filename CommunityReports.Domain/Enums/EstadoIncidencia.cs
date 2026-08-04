namespace CommunityReports.Domain.Enums
{
    /// <summary>
    /// Estado del ciclo de vida de una incidencia. Se modela como enum (y no como
    /// tabla "estado") por la misma razón que <see cref="TipoInfraestructura"/>: es
    /// un catálogo fijo y cerrado. Queda lista para cuando se implemente el módulo
    /// de Incidencia.
    /// </summary>
    public enum EstadoIncidencia
    {
        Reportada = 1,
        EnProceso,
        Resuelta,
        Rechazada,
        Cerrada
    }
}
