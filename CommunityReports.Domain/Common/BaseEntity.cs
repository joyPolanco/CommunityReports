namespace CommunityReports.Domain.Common
{
    /// <summary>
    /// Raíz común para todas las entidades del dominio. Centraliza la identidad (Id)
    /// para que EF Core pueda mapear la clave primaria sin exponer un setter público
    /// que permita a capas externas mutar la identidad de una entidad ya persistida.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }

        protected BaseEntity()
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity other) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (Id == 0 || other.Id == 0) return false;

            return Id == other.Id;
        }

        public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
    }
}
