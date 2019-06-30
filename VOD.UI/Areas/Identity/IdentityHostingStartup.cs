using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(VOD.UI.Areas.Identity.IdentityHostingStartup))]
namespace VOD.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}