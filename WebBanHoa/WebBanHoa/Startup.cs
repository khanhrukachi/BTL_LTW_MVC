using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBanHoa.Startup))]
namespace WebBanHoa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
