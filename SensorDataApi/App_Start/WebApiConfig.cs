using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace SensorDataApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger()); // 
            // Web API routes
            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute("http://localhost:4200,http://sensordataapp.azurewebsites.net", "*", "*");
            config.EnableCors(cors);
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // config.Routes.MapHttpRoute(
            //    name: "NewDataSources",
            //    routeTemplate: "api/{controller}/{action}"
            //);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{dataSource}",
                defaults: new { dataSource = UrlParameter.Optional }
            );
        }
    }
}
