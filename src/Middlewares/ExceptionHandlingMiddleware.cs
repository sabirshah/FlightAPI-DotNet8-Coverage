using System.Net;
using System.Text.Json;
namespace FlightInformationAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // pass control to next middleware  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                object errorResponse;
                if (_env.IsDevelopment())
                {
                    errorResponse = new
                    {
                        Message = "An unexpected error occurred.",
                        Details = ex.Message,
                        StackTrace = ex.StackTrace
                    };
                }
                else
                {
                    errorResponse = new
                    {
                        Message = "An unexpected error occurred. Please try again later."
                    };
                }

                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }

}
