using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Szl.MvcDemo.Szl;

namespace Szl.MvcDemo.Controllers
{
    public class BaseController : Controller
    {

        public EnrollmentState EnrollmentState
        {
            get { return VeryImpressiveInMemoryDatabase.EnrollmentState; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (UserClickedBack())
            {
                filterContext.Result = Back();
                filterContext.Result.ExecuteResult(ControllerContext);
                return;
            }


            var state = GetCurrentBoundAction();

            var stateMachine = GetStateMachine();
            var attemptedState = stateMachine.GetStateForAction(state);
            if (attemptedState == null &&
                !(state.Action == "Route" && state.Controller == "Route"))
            {
                filterContext.Result = Redirect("/route");
                filterContext.Result.ExecuteResult(ControllerContext);
                return;
            }
        }

        private BoundAction GetCurrentBoundAction()
        {
            var action = new BoundAction
            {
                Action = ControllerContext.RouteData.Values["action"].ToString(),
                Controller = ControllerContext.RouteData.Values["controller"].ToString(),
                Params = HttpContext.Request.QueryString.ToRouteValueDictionary()
            };
            return action;
        }

        private DemoStateMachine GetStateMachine()
        {
            return StateMachineBuilder.GetStateMachine(EnrollmentState);
        }

        protected virtual bool UserClickedBack()
        {
            var backButtonClicked = ControllerContext.RequestContext.HttpContext.Request.Form["BackButtonClicked"];
            bool retval;
            return bool.TryParse(backButtonClicked, out retval) && retval;
        }

        protected virtual RedirectToRouteResult Next()
        {
            var stateMachine = GetStateMachine();

            var currentAction = GetCurrentBoundAction();
            var currentState = stateMachine.GetStateForAction(currentAction);

            var nextState = (ActionableSzl)stateMachine.GetNextState(currentState);
            var nextAction = nextState.BoundAction;

            return RedirectToAction(nextAction.Action, nextAction.Controller, nextAction.Params);
        }

        protected virtual RedirectToRouteResult Back()
        {
            var stateMachine = GetStateMachine();

            var currentAction = GetCurrentBoundAction();
            var currentState = stateMachine.GetStateForAction(currentAction);

            var previousState = (ActionableSzl)stateMachine.GetPreviousState(currentState);
            var previousAction = previousState.BoundAction;

            return RedirectToAction(previousAction.Action, previousAction.Controller, previousAction.Params);
        }

        protected virtual RedirectToRouteResult RouteToFurthest()
        {
            var stateMachine = GetStateMachine();

            var state = (ActionableSzl)stateMachine.GetFurthestState();
            var action = state.BoundAction;

            return RedirectToAction(action.Action, action.Controller, new RouteValueDictionary(action.Params));
        }
    }
}