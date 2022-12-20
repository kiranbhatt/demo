using Books.API.BackgroundJob;
using Books.API.Configure;
using Books.API.Contexts;
using Books.API.Entities;
using Books.API.Middlewares;
using Books.Core.Configure;
using Books.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;

namespace Books.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCores(Configuration);

            // Added AddNewtonsoftJson() for HTTPPUT verb
            services.AddControllers().AddNewtonsoftJson();

            services.ConfigureAppRepositories(Configuration);

            services.ConfigureAppServices(Configuration);

            // Adding Identity

            /* Note : This line should always be above of -- services.AddAuthentication() otherwise JwtBearerDefaults.AuthenticationScheme will be ignored.
             *        AddIdentity() uses cookie based Authentication as default scheme. Otherwise your web-api-core-returns-404-when-adding-authorize-attribute.
             *        
             *        For validating JWT,JwtBearerDefaults.AuthenticationScheme is required.
             *        https://stackoverflow.com/questions/52038054/web-api-core-returns-404-when-adding-authorize-attribute
             */
            services.AddIdentity<ApplicationUser, ApplicationRole>(o => o.User.RequireUniqueEmail = true).AddEntityFrameworkStores<BookContext>();

            services.ConfigureJwtAuthentication(Configuration);

            services.AddHttpContextAccessor();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.ConfigureSwagger(Configuration);


            /* .AddMicrosoftIdentityWebApi(Configuration); Add this line to Protect the WebAPI using Microsoft Identity (Azure AD). This code itself will do token validation.*/

            /*
             * 
             * 
             *------------ Protect the WebAPI using Microsoft Identity (Azure AD). This code itself will do token validation.
             * 
             * 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration);
            */

            services.AddHostedService<BookBackgroundService>();


            // Add API versioning
            services.AddApiVersioning(option =>
            {
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });


            services.AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true; // This will show the version in URL
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CORS");

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {

                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Books Services  V1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Books Services  V2");


            });
        }
    }
}
