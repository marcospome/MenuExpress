using System;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MenuExpress.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Check if the response is a validation problem
                if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest &&
                    context.Response.ContentType == "application/problem+json")
                {
                    await HandleValidationProblemAsync(context);
                }
            }
            catch (JsonException jsonEx)
            {
                await HandleJsonExceptionAsync(context, jsonEx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleValidationProblemAsync(HttpContext context)
        {
            if (context.Response.HasStarted) return;

            context.Response.ContentType = "application/json";
            var originalResponseBodyStream = context.Response.Body;

            using var responseBody = new System.IO.MemoryStream();
            context.Response.Body = responseBody;

            // Copy the response body to memory stream
            await responseBody.CopyToAsync(originalResponseBodyStream);
            context.Response.Body.Position = 0;

            // Read the response content
            var responseContent = await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Position = 0;

            // Log the validation errors
            _logger.LogWarning("Validation errors occurred: {Response}", responseContent);

            var response = new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Por favor verifique los datos enviados.",
                Details = responseContent // Include original validation errors
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }

        private async Task HandleJsonExceptionAsync(HttpContext context, JsonException jsonEx)
        {
            if (context.Response.HasStarted) return;

            _logger.LogError(jsonEx, "Invalid JSON sent.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Json enviado es inválido."
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted) return;

            _logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Manejo de SqlException
            switch (ex)
            {
                case SqlException sqlEx:
                    context.Response.StatusCode = sqlEx.Number switch
                    {
                        547 => (int)HttpStatusCode.BadRequest, // Violación de clave foránea
                        2627 => (int)HttpStatusCode.Conflict,  // Violación de clave única
                        _ => (int)HttpStatusCode.InternalServerError // Otros errores de SQL
                    };

                    var sqlResponse = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = sqlEx.Number == 547
                            ? "Error: Se produjo un conflicto con la clave foránea. Verifica los datos proporcionados."
                            : "Error de base de datos. Verifica la consulta."
                    };

                    var sqlJsonResponse = JsonSerializer.Serialize(sqlResponse);
                    await context.Response.WriteAsync(sqlJsonResponse);
                    return; // Termina el método aquí para evitar la respuesta de error interno

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden; // Cambia a 403 Forbidden
                    var unauthorizedResponse = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Acceso prohibido."
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(unauthorizedResponse));
                    return;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    var notFoundResponse = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Recurso no encontrado."
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(notFoundResponse));
                    return;

                    // Puedes agregar más casos según sea necesario.
            }

            // Respuesta para errores generales
            var defaultResponse = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Ocurrió un error interno en el servidor."
            };

            var defaultJsonResponse = JsonSerializer.Serialize(defaultResponse);
            await context.Response.WriteAsync(defaultJsonResponse);
        }

    }
}
