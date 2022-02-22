using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookALookProject.Startup))]
namespace BookALookProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
