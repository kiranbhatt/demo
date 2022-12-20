using Books.API.Contexts;
using Books.API.Entities;
using Books.Core.Entities;
using Books.Core.Seeds;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Books.API
{
    public class Program
    {
        /// <summary>
        /// https://www.youtube.com/watch?v=_iryZxv8Rxw
        /// https://github.com/serilog/serilog-aspnetcore/blob/dev/samples/Sample/Program.cs
        /// https://www.youtube.com/watch?v=a68X_9CuUkw
        /// </summary>
        /// <param name="args"></param>
        public async static Task Main(string[] args)
        {

            //Read Configuration from appSettings    
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //Initialize Logger    
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var bookContext = services.GetRequiredService<BookContext>();
                    await bookContext.Database.MigrateAsync(); // Apply pending migration or create DB.
                    await Seed.SeedBooks(bookContext);

                    await Seed.SeedUsers(bookContext);

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    
                }

                    Log.Information("Application Starting.");

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start." + ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseSerilog() //Uses Serilog instead of default .NET Logger 
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
