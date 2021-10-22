using Common.Cripto;
using Common.Domain.Interfaces;
using Common.Domain.Model;
using Common.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Core.Api.Config
{
    public static partial class ConfigContainerCore
    {
        public static void RegisterOtherComponents(IServiceCollection services)
        {
            services.AddScoped<IEmail, Email>();
            services.AddScoped<IEmailApi, EmailSendGridApi>();

            services.AddScoped<ICripto, Cripto>();

            services.AddScoped<CurrentUser>();
        }
    }
}
