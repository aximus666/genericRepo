using Owin;
using Microsoft.Owin;
using Practice.Web;


[assembly: OwinStartup(typeof(Startup))]
namespace Practice.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

    

        }

    
    }
}