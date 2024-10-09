using Newtonsoft.Json;
using System.Net;

namespace api.Middlewares
{
    public class ExceptionHandlingMiddlewareT2
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;


        public ExceptionHandlingMiddlewareT2(RequestDelegate next, ILogger<ExceptionHandlingMiddlewareT2> logger)
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
                await HandleException(context, ex);
            }

            return;
        }


        private Task HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Custom Message = Internal Server Error",
                Detailed = ex.Message,
                Edditional_Fields1 = "Edditonal FIelds 1",
                Edditional_Fields2 = "Edditonal FIelds 2"
            };

            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
