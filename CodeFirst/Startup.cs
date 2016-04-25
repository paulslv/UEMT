using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeFirst.Startup))]
namespace CodeFirst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app1);
        }
    }
}
