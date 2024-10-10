using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace api.Middlewares
{
    public class ExceptionHandlingMiddlewareT3 : IMiddleware
    {
        #region Initialization

        private readonly ILogger<ExceptionHandlingMiddlewareT3> _logger;

        public ExceptionHandlingMiddlewareT3(ILogger<ExceptionHandlingMiddlewareT3> logger)
        {
            _logger = logger;
        }

        #endregion



        #region Implementation

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                ProblemDetails details = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "Error!!!",
                    Detail = ex.Message,
                };

                context.Response.ContentType = "application/json";
                string json = JsonSerializer.Serialize(details);
                await context.Response.WriteAsync(json);
            }

            return;
        }

        #endregion
    }
}
