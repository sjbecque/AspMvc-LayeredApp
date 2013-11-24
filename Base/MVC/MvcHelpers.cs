//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;
//using System.Web.Routing;
//using System.ComponentModel;

//namespace Base.MVC
//{
//    public static class UrlHelperExtensions
//    {
//        //http://www.zvolkov.com/clog/2012/04/01/asp-net-mvc-build-url-based-on-current-url/
//        //Builds URL by finding the best matching route that corresponds to the current URL,
//        //with given parameters added or replaced.
//        public static MvcHtmlString Current(this UrlHelper helper, object substitutes)
//        {
//            //get the route data for the current URL e.g. /Research/InvestmentModelling/RiskComparison
//            //this is needed because unlike UrlHelper.Action, UrlHelper.RouteUrl sets includeImplicitMvcValues to false
//            //which causes it to ignore current ViewContext.RouteData.Values
//            var rd = new RouteValueDictionary(helper.RequestContext.RouteData.Values);
 
//            //get the current query string e.g. ?BucketID=17371&compareTo=123
//            var qs = helper.RequestContext.HttpContext.Request.QueryString;
 
//            //add query string parameters to the route value dictionary
//            foreach (string param in qs)
//                if(!string.IsNullOrEmpty(qs[param]))
//                    rd[param] = qs[param];
 
//            //override parameters we're changing
//            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties( substitutes.GetType( ) ))
//            {
//                var value = property.GetValue( substitutes );
//                if (value == null || value.ToString( ) == string.Empty)
//                {
//                    rd.Remove( property.Name );
//                }
//                else
//                {
//                    rd[property.Name] = value;
//                }
//            }
//            //UrlHelper will find the first matching route
//            //(the routes are searched in the order they were registered).
//            //The unmatched parameters will be added as query string.
//            var url = helper.RouteUrl(rd);
//            return new MvcHtmlString(url);
//        }
//    }

//    public class New
//    {
//        public static SelectListItem SelectListItem( string value, string text, bool selected )
//        {
//            return new SelectListItem { Value = value, Text = text, Selected = selected };
//        }
//    }
//}
