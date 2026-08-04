using System.Net;
using System.Text.Json;
using CommunityReports.Application.Exceptions;
using FluentValidation;

namespace CommunityReports.Api.Middleware
{
    /// <summary>
    /// Traduce excepciones de Application (y de validación) a respuestas HTTP
    /// consistentes, para que los controladores no necesiten try/catch repetitivos.
    /// </summary>
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleAsync(context, ex);
            }
        }

        private async Task HandleAsync(HttpContext context, Exception exception)
        {
            var (statusCode, errors) = exception switch
            {
                ValidationException validationException => (
                    HttpStatusCode.BadRequest,
                    (object)validationException.Errors.Select(e => e.ErrorMessage)),

                NotFoundAppException => (HttpStatusCode.NotFound, (object)new[] { exception.Message }),
                ConflictAppException => (HttpStatusCode.Conflict, (object)new[] { exception.Message }),
                UnauthorizedAppException => (HttpStatusCode.Unauthorized, (object)new[] { exception.Message }),
                ArgumentException => (HttpStatusCode.BadRequest, (object)new[] { exception.Message }),

                _ => (HttpStatusCode.InternalServerError, (object)new[] { "Ocurrió un error inesperado." })
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Error no controlado");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var body = JsonSerializer.Serialize(new { errores = errors });
            await context.Response.WriteAsync(body);
        }
    }
}
