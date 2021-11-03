using AutoMapper;
using DatingApp.Data;
using DatingApp.Helper;
using DatingApp.Interfaces;
using DatingApp.Services;
using DatingApp.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Extensions
{
    public static class AddApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services , IConfiguration config)
        {
            services.AddDbContext<Entities.DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DatingApp.Helper.AutoMapperProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton<PresenceTracker>();
            services.AddSingleton(mapper);
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySetting"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
           
            return services;
        }
    }
}
