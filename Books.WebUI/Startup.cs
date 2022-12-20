using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Books.WebUI
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
            services.AddMicrosoftIdentityWebAppAuthentication(Configuration)
             .EnableTokenAcquisitionToCallDownstreamApi(new string[] {
                 Configuration["AzureAd:Scopes:Book.Create"], 
                 Configuration["AzureAd:Scopes:Book.Read"],
                 Configuration["AzureAd:Scopes:Book.ReadAll"]
             })
            .AddInMemoryTokenCaches();

            services.AddControllersWithViews();

            services.AddRazorPages().AddMvcOptions(options => 
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.Filters.Add(new AuthorizeFilter(policy));

            }).AddMicrosoftIdentityUI();


            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // This is required for SignIn & SignOut. Otherwise URL will not be work
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
