using System.Net;
using Newtonsoft.Json;
using Aplicacion.Exceptions;
using Aplicacion.Wrappers;

namespace InventoryApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private string? body;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                var reader = new StreamReader(context.Request.Body);
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (string.IsNullOrEmpty(body) && context.Request.QueryString.HasValue)
                {
                    body = context.Request.QueryString.Value;
                }

                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error.Message };

                switch (error)
                {
                    case ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonConvert.SerializeObject(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
