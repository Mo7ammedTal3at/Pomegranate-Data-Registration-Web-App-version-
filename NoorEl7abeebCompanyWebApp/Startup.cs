using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NoorEl7abeebCompanyWebApp.Startup))]
namespace NoorEl7abeebCompanyWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
