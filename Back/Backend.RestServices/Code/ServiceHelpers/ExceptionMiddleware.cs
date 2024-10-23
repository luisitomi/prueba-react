using Backend.CrossCuting.Helpers;
using Backend.RestServices.Code.Log;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Backend.RestServices.Code.ServiceHelpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private readonly AppConfiguration _configuration;
        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _logger = logger;
            _next = next;
            _configuration = new AppConfiguration();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var fullFilePath = _configuration._PathFile + "log_{date}.log".Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss"));
                var logRecord = string.Format("{0} [{1}] {2}", "[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]", httpContext, ex);

                if (!(Directory.Exists(fullFilePath)))
                {
                    Directory.CreateDirectory(_configuration._PathFile);
                }

                using (var streamWriter = new StreamWriter(fullFilePath, true))
                {
                    streamWriter.WriteLine(logRecord);
                }

                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
               StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }

    }
}
