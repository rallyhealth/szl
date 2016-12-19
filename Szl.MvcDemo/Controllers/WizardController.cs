using System.Web.Mvc;

namespace Szl.MvcDemo.Controllers
{
    public class WizardController : BaseController
    {
        public ActionResult Step1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Step1(Step1ViewModel viewModel)
        {
            return Next();
        }

        public ActionResult Step2()
        {
            return View(new Step2ViewModel {UnblockBlockNodes = EnrollmentState.AllowBlockedNode});
        }

        [HttpPost]
        public ActionResult Step2(Step2ViewModel viewModel)
        {
            EnrollmentState.AllowBlockedNode = viewModel.UnblockBlockNodes;
            EnrollmentState.Dirty = true;
            return Next();
        }

        public ActionResult Step3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Step3(Step3ViewModel viewModel)
        {
            EnrollmentState.NumberOfPrograms = viewModel.NumberOfPrograms;
            EnrollmentState.Dirty = true;
            return Next();
        }
    }

    public class Step3ViewModel
    {
        public int NumberOfPrograms { get; set; }
    }

    public class Step2ViewModel
    {
        public bool UnblockBlockNodes { get; set; }
    }

    public class Step1ViewModel
    {

    }
}