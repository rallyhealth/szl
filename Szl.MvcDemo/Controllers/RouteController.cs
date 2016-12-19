using System.Web.Mvc;
using System.Web.Routing;

namespace Szl.MvcDemo.Controllers
{
    public class RouteController : BaseController
    {
        [Route("route")]
        public ActionResult Route()
        {
            return RouteToFurthest();
        }
    }
}