using FRN.Application._1._1_Interface;
using FRN.Application._1._2_AppService;
using FRN.Domain._2._1_Interface;
using FRN.Domain.Notifications;
using FRN.Infra._3._1_Context;
using FRN.Infra._3._3_Repository;
using Microsoft.Extensions.DependencyInjection;

namespace FRN.Infra.CrossCutting.Ioc
{
    public static class BootStrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services) 
        {            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IDomainNotificationHandler, DomainNotificationHandler>();           

            services.AddScoped<FRNContext>();

            return services;
        }
    }
}
