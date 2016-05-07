using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(EatThemAll.Server.Startup))]

namespace EatThemAll.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            // SignalR
            //app.MapSignalR();
            //app.Map("/signalr", appBuilder =>
            //{
            //    appBuilder.UseCors(CorsOptions.AllowAll);
            //    appBuilder.RunSignalR(new HubConfiguration());
            //});
        }
    }
}
