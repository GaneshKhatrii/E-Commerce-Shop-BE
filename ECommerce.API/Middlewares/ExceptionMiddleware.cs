using ECommerce.Application.Common;
using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Data = null
                };

                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            }
            //catch (Exception ex)
            //{
            //    var actualMessage = ex.InnerException?.Message ?? ex.Message;

            //    var response = new ApiResponse<string>
            //    {
            //        Success = false,
            //        StatusCode = 400,
            //        Message = actualMessage
            //    };

            //    await context.Response.WriteAsJsonAsync(response);
            //}
        }
    }
}
