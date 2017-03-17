# Szl.MvcDemo

A sample usage of Szl in an ASP.NET MVC application.

This application provides a simple wizard with Szl states which are dependent on the user's application state.

The user by default has access to "Step 1" and "Step 2". The user only has access to "Step 3" if they check the "Access blocked nodes" box. Following that, the user can select a number of programs they want to navigate through. This results in a dynamic number of child wizards added to the user's state and they can navigate forward and back through them.

# `DemoStateMachine`

A concrete implementation of `SzlBase` to act as the class definition of the state machine.

# `StateMachineBuilder.GetStateMachine`

A state machine factory (and pure function) that takes in the user's `EnrollmentState` and returns the state machine (an instance of `DemoStateMachine`) defining all of their possible states.

# `StateMachineBuilder.GetWizardSzls` and `StateMachineBuilder.GetProgramSzls`

Helper methods that build up different subsets of the state machine. These demonstrate that the logic of building the state machine can be modular and deal with subsets of the total application context.

# `EnrollmentState`

A container for the user's context. This is what drives the dynamic Szl states.

# `BaseController`

`BaseController` is what ties Szl to the MVC application. All Szl-managed controllers inherit from `BaseController` and rely on `OnActionExecuting`, `Next`, and `Back` for navigation and preventing improper application state access.

Following the [https://en.wikipedia.org/wiki/Post/Redirect/Get](Post/Redirect/Get) pattern of MVC applications, `POST` actions are responsible for redirecting to the next action. `Next` in the `BaseController` shifts the responsibility of figuring out where to go **from** the action itself **to** Szl. `Next` uses the `ControllerContext.RouteData` to determine the current controller and action and find the next `Szl` state and associated MVC action.

`OnActionExecuting` is an MVC lifecycle event that fires before an action is executed. Here, we assert that a user is allowed to access the action they're attempting. It created a `BoundAction` based on their attempted action, looks for the `Szl` state in the state machine, and if it's null, routes them elsewhere.