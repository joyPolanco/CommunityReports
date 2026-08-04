namespace CommunityReports.Application.Exceptions
{
    /// <summary>
    /// Base para excepciones de aplicación que la Api traduce a códigos HTTP
    /// específicos, evitando que los controladores conozcan detalles del dominio.
    /// </summary>
    public abstract class AppException : Exception
    {
        protected AppException(string message) : base(message)
        {
        }
    }

    public sealed class NotFoundAppException : AppException
    {
        public NotFoundAppException(string message) : base(message)
        {
        }
    }

    public sealed class ConflictAppException : AppException
    {
        public ConflictAppException(string message) : base(message)
        {
        }
    }

    public sealed class UnauthorizedAppException : AppException
    {
        public UnauthorizedAppException(string message) : base(message)
        {
        }
    }
}
