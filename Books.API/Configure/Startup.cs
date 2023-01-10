using Books.API.Contexts;
using Books.API.Services;
using Books.API.Services.Abstract;
using Books.Core.Repositories.Implementation.Dapper;
using Books.Core.Repositories.Implementation.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.API.Configure
{
    public static class Startup
    {

        /// <summary>Configure App Services</summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        public static void ConfigureAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            // http request should be AddScoped

           
            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped<IRolesService, RolesService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();                
                      
        }      
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;

                    o.IncludeErrorDetails = true;

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]")),

                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        }

        /// <summary>
        /// SwaggerDoc("v1", options.SwaggerDoc("v2"   must match with value in {{}}
        /// 
        ///   options.SwaggerEndpoint("/swagger/ {{v1}}/swagger.json", "Books Services  V1");
        ///   options.SwaggerEndpoint("/swagger/{{v2}}/swagger.json", "Books Services  V2");
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Books Info Service API v1",
                    Version = "v1.0",
                    Description = "Sample service for Books",
                });

                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Books Info Service API v2",
                    Version = "v2.0",
                    Description = "Sample service for Books",
                });
            });

        }


        public static void ConfigureCores(this IServiceCollection services, IConfiguration configuration)
        {
            string MyAllowSpecificOrigins = "CORS";

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                       policy =>
                                       {
                                           policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                       });
            });

        }
    }
}


