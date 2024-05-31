using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;

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

                var stopwatch = Stopwatch.StartNew();
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    stopwatch.Stop();
                    var requestTime = stopwatch.Elapsed.TotalMilliseconds;

                    LogRequestTimeToFile(context, requestTime);

                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }


        private void LogRequestTimeToFile(HttpContext context, double requestTime)
        {
            var logMessage = $"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} - Request: {context.Request.Path} completed in {requestTime} ms\n";
            var logFilePath = "logs.txt"; // Specify your log file path here

            // Write log message to file
            try
            {
                using (var logFileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    var logBytes = Encoding.UTF8.GetBytes(logMessage);
                    logFileStream.Write(logBytes, 0, logBytes.Length);
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., logging it)
                _logger.LogError($"Failed to write to log file: {ex.Message}");
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
