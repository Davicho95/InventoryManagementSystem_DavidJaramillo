using InventoryApi.Middlewares;

namespace InventoryApi.Extensiones
{
    public static class ExtensionesApp
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
