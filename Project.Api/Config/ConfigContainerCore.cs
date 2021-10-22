using Common.Domain.Interfaces;
using Common.Orm;
using Common.Validation;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Services;
using Project.Core.Data.Repository;
using Microsoft.Extensions.Options;
using Project.Data.Context;

namespace Project.Core.Api.Config
{
    public static partial class ConfigContainerCore
    {
        public static void Config(IServiceCollection services)
        {
            services.AddScoped<DbContextMongoDb>();
            services.AddScoped<ValidationContract>();

            services.AddScoped<UsuarioRepository>();
            services.AddScoped<UsuarioService>();

            
            RegisterOtherComponents(services);
        }
    }
}
