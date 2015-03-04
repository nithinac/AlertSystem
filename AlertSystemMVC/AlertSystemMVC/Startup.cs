using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;

namespace AlertSystemMVC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfire(config =>
            {
                config.UseSqlServerStorage("AlertSystemMVCContext");
                config.UseServer();
            });
        }
    }
}