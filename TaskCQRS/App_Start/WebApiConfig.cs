using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace TaskCQRS
{
    public static class WebApiConfig
    {
        
        public static void Register(HttpConfiguration config)
        {
            // NOTE: Serialize Enums as strings but not as number values.
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var enumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
            jsonFormatter.SerializerSettings.Converters.Add(enumConverter);

            config.Routes.MapHttpRoute(
                name: "services",
                routeTemplate: "services/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "servicesDeleteWithVersion",
                routeTemplate: "services/{controller}/{id}/version={version}",
                defaults: null,
                constraints: new { id = new AllowedForMethod("DELETE") }
            );
        }
    }
    public class AllowedForMethod : IRouteConstraint
    {
        readonly string method;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllowedForMethod" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public AllowedForMethod(string method)
        {
            this.method = method;
        }

        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that this constraint belongs to.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for the URL.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        /// <returns>
        /// true if the URL parameter contains a valid value; otherwise, false.
        /// </returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.IncomingRequest &&
                httpContext.Request.HttpMethod != this.method &&
                values[parameterName] != null)
            {
                return false;
            }

            return true;
        }
    }
}
