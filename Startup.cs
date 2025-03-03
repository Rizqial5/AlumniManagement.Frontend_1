using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlumniManagement.Frontend.Startup))]
namespace AlumniManagement.Frontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
