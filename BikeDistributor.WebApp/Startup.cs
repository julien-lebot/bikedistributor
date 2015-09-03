using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using MVCControlsToolkit.Owin.Globalization;
using Owin;

[assembly: OwinStartup(typeof(BikeDistributor.WebApp.Startup))]

namespace BikeDistributor.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseGlobalization(new OwinGlobalizationOptions("en-US", true)
               .DisablePaths("Content", "bundles", "api", "__browserLink")
               .Add("fr-FR", true).AddCustomSeeker(new CultureFromPreferences()));
        }
    }
}
