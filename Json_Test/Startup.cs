using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Json_Test.Startup))]
namespace Json_Test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
