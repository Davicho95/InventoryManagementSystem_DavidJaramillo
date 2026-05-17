using Aplicacion.Interfaces;
using Aplicacion.Interfaces.Auth;
using Aplicacion.Wrappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Persistencia.Contexto;
using Persistencia.Services;
using Persistencia.Services.Auth;
using System.Text;

namespace Persistencia
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            string cadenaConexion = configuration.GetConnectionString("DefaultConnection")!;

            services.AddDbContext<AppDbContext>(options => options.UseLazyLoadingProxies(true).UseSqlServer(
                cadenaConexion, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Servicios
            services.AddScoped<IAuthService, AuthServiceAsync>();
            services.AddScoped<IProductoRepositoryAsync, ProductoRepositoryAsync>();
            services.AddScoped<IMovimientoStockRepositoryAsync, MovimientoStockRepositoryAsync>();

            #region Verifiacion Token
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                var key = configuration["JWTSettings:Key"]!;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

                o.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("Usted no esta autorizado"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("Usted no tiene permisos sobre este recurso"));
                        return context.Response.WriteAsync(result);
                    }
                };
            });
            #endregion
        }
    }
}
