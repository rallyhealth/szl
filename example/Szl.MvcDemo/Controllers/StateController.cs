using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Szl.MvcDemo.Controllers
{
    public class StateController : Controller
    {
        [Route("reset")]
        public ActionResult Reset()
        {
            VeryImpressiveInMemoryDatabase.Guids = new List<Guid>();
            var state = VeryImpressiveInMemoryDatabase.EnrollmentState;
            state.AllowBlockedNode = false;
            state.NumberOfPrograms = 0;
            state.Dirty = true;

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}