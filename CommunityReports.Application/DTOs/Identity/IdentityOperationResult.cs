namespace CommunityReports.Application.DTOs.Identity
{
    /// <summary>
    /// Resultado de una operación contra el proveedor de identidad (crear usuario,
    /// cambiar contraseña, etc.), traducido a un contrato propio de Application para
    /// no exponer <c>IdentityResult</c> de Microsoft.AspNetCore.Identity fuera de
    /// Infrastructure.
    /// </summary>
    public class IdentityOperationResult
    {
        public bool Succeeded { get; init; }
        public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

        public static IdentityOperationResult Ok() => new() { Succeeded = true };
        public static IdentityOperationResult Fail(IEnumerable<string> errors) =>
            new() { Succeeded = false, Errors = errors.ToList() };
        public static IdentityOperationResult Fail(string error) => Fail(new[] { error });
    }

    /// <summary>Variante con un valor de resultado (por ejemplo, el id del usuario creado).</summary>
    public sealed class IdentityOperationResult<T> : IdentityOperationResult
    {
        public T? Value { get; init; }

        public static IdentityOperationResult<T> Ok(T value) => new() { Succeeded = true, Value = value };
        public static new IdentityOperationResult<T> Fail(IEnumerable<string> errors) =>
            new() { Succeeded = false, Errors = errors.ToList() };
        public static new IdentityOperationResult<T> Fail(string error) => Fail(new[] { error });
    }
}
