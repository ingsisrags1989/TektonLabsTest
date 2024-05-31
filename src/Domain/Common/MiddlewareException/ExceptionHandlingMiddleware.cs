using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Domain.Common.MiddlewareException
{
    public class ExceptionHandlingMiddleware
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
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = exception.Message;

            if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            context.Response.StatusCode = (int)statusCode;
            var response = JsonConvert.SerializeObject(new ResponseStatusCode
            {
                StatusCode = context.Response.StatusCode,
                Message = message ?? ""
            });
            return context.Response.WriteAsync(response);
        }
    }
}
