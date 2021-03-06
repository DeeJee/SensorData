﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc.Filters;

namespace SensorDataApi.Attributes
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent("use https instead of http");

                UriBuilder builder = new UriBuilder(actionContext.Request.RequestUri);
                builder.Scheme = Uri.UriSchemeHttps;
#if DEBUG
                builder.Port = 44374;
#endif

                actionContext.Response.Headers.Location = builder.Uri;
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}
