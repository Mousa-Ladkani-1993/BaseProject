using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseProjectApp.API.Middlewares.ExceptionHandler
{
    public class ErrorHandlerMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {

                var st = new StackTrace(error, true); 
                var frame = st.GetFrame(0);
                var fileName = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = APIResponse<string>.Fail(error.Message + fileName+"- line:" + line, "");  

                switch (error)
                {
                    case CustomException e: 
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e: 
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default: 
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }  
                
                _logger.LogError(error.Message);

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
