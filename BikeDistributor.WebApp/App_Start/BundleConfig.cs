using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace BikeDistributor.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://code.jquery.com/jquery-2.1.4.min.js").Include(
                "~/wwwroot/lib/jquery/dist/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular", "https://ajax.googleapis.com/ajax/libs/angularjs/1.4.5/angular.min.js").Include(
                "~/wwwroot/lib/angularjs/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-route", "https://ajax.googleapis.com/ajax/libs/angularjs/1.4.5/angular-route.js").Include(
                "~/wwwroot/lib/angular-route/angular-route.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/wwwroot/js/app/app.js",
                "~/wwwroot/js/app/controllers/*.js",
                "~/wwwroot/js/app/services/*.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/wwwroot/lib/jquery.unobtrusive*",
                "~/wwwroot/lib/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/wwwroot/lib/modernizr/modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/wwwroot/lib/bootstrap/dist/js/bootstrap.min.js",
                "~/wwwroot/lib/respond/dest/respond.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/wwwroot/lib/bootstrap/dist/css/bootstrap.min.css",
                 "~/wwwroot/css/Site.css"));
        }
    }
}
