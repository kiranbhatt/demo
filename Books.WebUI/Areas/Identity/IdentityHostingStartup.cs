using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Books.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace Books.WebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //builder.ConfigureServices((context, services) => {
            //    services.AddDbContext<BooksWebUIContext>(options =>
            //        options.UseSqlite(
            //            context.Configuration.GetConnectionString("BooksWebUIContextConnection")));

            //    services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //        .AddEntityFrameworkStores<BooksWebUIContext>();
            //});
        }
    }
}