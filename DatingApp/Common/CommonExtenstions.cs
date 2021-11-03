using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Common
{
    public static class CommonExtenstions
    {


        public static IServiceCollection AddSwaggerDocumentation_Old(this IServiceCollection services, string documentName, string title, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(documentName,
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = title,
                        Version = version
                    }
                );
            });

            return services;
        }


        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, string documentName, string title, string Version)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/" + documentName + "/swagger.json", documentName);

                c.DocumentTitle = title;
                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }


    }
}
