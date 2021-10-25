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

            services.AddScoped<CategoriaRepository>();
            services.AddScoped<CategoriaService>();
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<ProdutoRepository>();
            services.AddScoped<ProdutoService>();
            
            RegisterOtherComponents(services);
        }
    }
}
