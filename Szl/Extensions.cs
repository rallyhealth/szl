using System.Collections.Specialized;
using System.Linq;
using System.Web.Routing;

namespace Szl
{
    public static class Extensions
    {
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection nameValueCollection)
        {
            var routeValueDictionary = new RouteValueDictionary();
            var keys = nameValueCollection.AllKeys.Where(key => key != null);
            foreach (var key in keys)
            {
                routeValueDictionary[key] = nameValueCollection[key];
            }
            return routeValueDictionary;
        }
    }
}
