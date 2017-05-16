﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SensorDataManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/"               
           );
            config.Routes.MapHttpRoute(
                name: "SensorData",
                routeTemplate: "api/{controller}/{dataSource}/{van}/{tot}",
                defaults: new { van = RouteParameter.Optional, tot = RouteParameter.Optional }
            );
        }
    }
}
