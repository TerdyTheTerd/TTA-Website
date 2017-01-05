using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        public void ConfigureServices(ServiceCollection services)
        {
            
            //services.AddTransient<WebApplication1.Models.ForumsDbContext>();
        }
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
