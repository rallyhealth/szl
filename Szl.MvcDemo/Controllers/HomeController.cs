using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Szl.MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            VeryImpressiveInMemoryDatabase.Guids.Add(Guid.NewGuid());

            var model = new IndexModel
            {
                Message = "Index",
                Guids = VeryImpressiveInMemoryDatabase.Guids
            };
            ViewBag.IsIndex = true;
            return View(model);
        }
    }

    public class IndexModel
    {
        public string Message { get; set; }
        public List<Guid> Guids { get; set; }
    }
}