using InventoryMg.BLL.Implementation;
using InventoryMg.BLL.Interfaces;
using InventoryMg.DAL.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryMg.BLL.Extensions
{
    public static class ApplicationBuilderExtention
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder) 
            => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();

/*        public static void ConfigureLoggerService(this IServiceCollection services) => 
            services.AddSingleton<ILoggerManager, LoggerManager>();*/
    }
}
