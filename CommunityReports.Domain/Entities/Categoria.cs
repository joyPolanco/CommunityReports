using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    /// <summary>
    /// Categoría de incidencia (ej. "Alumbrado público", "Vías"), con color para la UI
    /// y un tiempo de respuesta esperado en horas, usado como SLA de referencia.
    /// </summary>
    public class Categoria : CatalogoBase
    {
        public string Color { get; private set; } = "#808080";
        public int TiempoRespuesta { get; private set; }

        private Categoria()
        {
        }

        public Categoria(string nombre, string color, int tiempoRespuestaHoras) : base(nombre)
        {
            EstablecerColor(color);
            EstablecerTiempoRespuesta(tiempoRespuestaHoras);
        }

        public void ActualizarDatos(string nombre, string color, int tiempoRespuestaHoras)
        {
            ActualizarNombre(nombre);
            EstablecerColor(color);
            EstablecerTiempoRespuesta(tiempoRespuestaHoras);
        }

        private void EstablecerColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
                throw new ArgumentException("El color es obligatorio.", nameof(color));

            Color = color.Trim();
        }

        private void EstablecerTiempoRespuesta(int horas)
        {
            if (horas <= 0)
                throw new ArgumentException("El tiempo de respuesta debe ser mayor a cero.", nameof(horas));

            TiempoRespuesta = horas;
        }
    }
}
