using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DatingApp.Extensions;
using DatingApp.Common;
using DatingApp.Middleware;
using DatingApp.SignalR;

namespace DatingApp
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(_config);
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder =>
                 builder.WithOrigins("http://localhost:4200")
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
            });
            services.AddIdentityServices(_config);
            services.AddSwaggerDocumentation_Old(_config.GetValue<string>("Swagger:DocumentName"), _config.GetValue<string>("Swagger:Title"), _config.GetValue<string>("Swagger:Version"));
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseMiddleware<ExceptionMiddelware>();
            app.UseSwaggerDocumentation(_config.GetValue<string>("Swagger:DocumentName"), _config.GetValue<string>("Swagger:Title"), _config.GetValue<string>("Swagger:Version"));
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapHub<MessageHub>("hubs/message");
               // endpoints.MapFallbackToController("Index", "Fallback");

            });
        }
    }
}
