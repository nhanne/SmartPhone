using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(Nike.Startup))]
namespace Nike
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
