using System.Net;

namespace MenuExpress.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);  // Continuar con el siguiente middleware
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error: {ex.Message}");

                // Verificar el tipo de excepción y manejarla adecuadamente
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Personalización de errores según el tipo de excepción
            switch (exception)
            {
                // Caso para errores de validación (por ejemplo, BadRequest - 400)
                case ArgumentException argEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Su pedido contiene errores, favor de validar nuevamente."  // Mensaje personalizado para 400
                    }));

                // Caso general para otros errores (Internal Server Error - 500)
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Ha ocurrido un error con su pedido, favor de intentar nuevamente más tarde."  // Mensaje personalizado para 500
                    }));
            }
        }
    }
}