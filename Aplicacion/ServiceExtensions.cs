using Microsoft.Extensions.DependencyInjection;

namespace Aplicacion
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly);
            });
        }
    }
}
