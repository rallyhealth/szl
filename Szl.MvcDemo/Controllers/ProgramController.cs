using System.Web.Mvc;

namespace Szl.MvcDemo.Controllers
{
    public class ProgramController : BaseController
    {
        public ActionResult Step1(int programId)
        {
            return View(new ProgramStep1ViewModel { ProgramId = programId});
        }

        [HttpPost]
        public ActionResult Step1(ProgramStep1ViewModel viewModel)
        {
            return Next();
        }

        public ActionResult Step2(int programId)
        {
            return View(new ProgramStep2ViewModel { ProgramId = programId });
        }

        [HttpPost]
        public ActionResult Step2(ProgramStep2ViewModel viewModel)
        {
            return Next();
        }

        public ActionResult Step3(int programId)
        {
            return View(new ProgramStep3ViewModel { ProgramId = programId });
        }

        [HttpPost]
        public ActionResult Step3(ProgramStep3ViewModel viewModel)
        {
            return Next();
        }
    }

    public class ProgramViewModel
    {
        public int ProgramId { get; set; }
    }

    public class ProgramStep3ViewModel : ProgramViewModel
    {

    }

    public class ProgramStep2ViewModel : ProgramViewModel
    {
    }

    public class ProgramStep1ViewModel : ProgramViewModel
    {

    }
}