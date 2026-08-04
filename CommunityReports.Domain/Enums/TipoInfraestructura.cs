namespace CommunityReports.Domain.Enums
{
    /// <summary>
    /// Clasificación de una <see cref="Entities.Infraestructura"/>. Se modela como
    /// enum (y no como tabla) porque es un catálogo fijo, cerrado y sin datos
    /// propios más allá del nombre — agregar un valor nuevo es un cambio de código,
    /// no una operación administrativa en runtime.
    /// </summary>
    public enum TipoInfraestructura
    {
        AlumbradoPublico = 1,
        Acueducto,
        Alcantarillado,
        EnergiaElectrica,
        Vialidad,
        SenalizacionVial,
        ParquesYAreasVerdes,
        RecoleccionDesechos,
        Otro
    }
}
