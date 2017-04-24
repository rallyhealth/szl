# Szl

Szl is a C# library for creating routing state machines. It includes a reference implementation for use in ASP.NET MVC-style applications, but the core library is adaptable for other potential implementations.

A Szl configuration answers the following questions:

- Given my context (am I logged in, have I answered a required question, etc.), can I go to X (where x is an application state or page)
- Given my context and that I am on State X, what state is Next?
- Given my context and that I am on State X, what state is Back?

A Szl configuration is a pure function (data in, data out, no side effects) and is highly testable because they are composite in nature and can be nested.

# Motivation and Usage

Rally Choice is an ASP.NET MVC application which walks a user through an Open Enrollment experience for employer-offered benefits. The main interaction is walking the user through a detailed wizard. The application states vary widely based on what an employee is offered, what they are eligible for, and even within a product, the flow changes based on their dependents' information.

Because this flow is very dynamic, it is no small effort to define all the possible states of an application can access, as well as answer the question "when a user is here, where should they go next?"

The previous implementation of routing in Choice essentially boiled down to a mess of `if` statements which were difficult to maintain and extend.

With Szl, the aim was to modularize the routing concerns of specific parts of the application as well as make them more testable.

# Concepts

The following terms make up the `Szl` library. See the [sample](#sample) concrete usages of these terms.

## `Szl`

A `Szl` instance is a tree composed of zero to many child `Szl` nodes.

Any given `Szl` can represent a grouping of child `Szl`s or an [Actionable Szl](#actionableszl), which represent concrete application states.

The root `Szl` acts as the navigation state machine for your application. See [a sample below](#sample) or the MvcDemo application.

All implementations of `ISzl` are considered `Szl`s.

## `BaseSzl`

`BaseSzl` is an abstract implementation of `ISzl` which implements the core functionality of the Szl library, including tree traversal and searching a `Szl` and its children for the `Szl` matching a given action.

`GetNextState` and `GetPreviousState` take in a `Szl` representing the current application state and search child `Szl`s for the next or previous actionable state respectively.

`GetStateForAction` take in an `object` representing the action associated with the current `Szl` application state and attempts to find a `Szl` associated with that action. In the case of the [`ActionableSzl`](#actionableszl), this is a [`BoundAction`](#boundaction).

## `ActionableSzl`

An implementation of `SzlBase` which represents a concrete application state (such as a step in a wizard).

An `ActionableSzl` state has one `BoundAction` property which includes metadata about the MVC controller and action that are associated with the state. This is how the Szl state machine is mapped to the states of an MVC application.

# Sample

To give the previous (relatively abstract) concepts some context, here is a minimal usage of Szl.

First, we define a concrete implementation of `SzlBase` that represents the state machine of the application.

```cs
public class DemoStateMachine : SzlBase
{
    public DemoStateMachine(ICollection<SzlBase> children) : base(children) { }
}
```

With that in place, we can now define a state machine factory method. This method should take in context about the user and their state in the application. For example, if they're an admin user, they might have access to more application states. Or, if the user hasn't completed the first step of the wizard, they *shouldn't* have access to the second step.

As we'll see, the states in the root state machine Szl represent all the states a user currently has access to. If the state they attempt to view is not in the state machine, they will not be able to access it.

```cs
public ISzl BuildStateMachine(bool, userIsLoggedIn, bool userIsAdmin = false)
{
    var states = new List<SzlBase>
    {
        // Actionable states in the application (identified by MVC controller/action)
        new ActionableSzl(controller: "Home", action: "Step1"),
        new ActionableSzl("Home", "Step2"),
        new ActionableSzl("Home", "Step3"),
    };

    // Change states based on user context (permissions, progression through application)
    if (userIsAdmin)
    {
        states.Add(new ActionableSzl("Home", "SecretStep"));
    }

    // Return instance of state machine with Szl states as its child nodes
    return new DemoStateMachine(states);
}
```

Now we have all we need to use our Szl state machine for routing. Here are a few scenarios:

**Given the user just POSTed a form from "Step2", where should they go next?**

```cs
//Note: Explicit variable types used for demonstration

// Build the state machine based on the user's context
ISzl stateMachine = BuildStateMachine(userIsLoggedIn: true);
// Create a `BoundActions` representing the current action
BoundAction currentBoundAction = new BoundAction("Home", "Step2");
// Search for the Szl state in the state machine associated with the current action
ISzl currentState = stateMachine.GetStateForAction(currentBoundAction);
// Search the state machine for the next state relative to the current state
ActionableSzl nextState = stateMachine.GetNextState(currentState) as ActionableSzl;
// Access the bound action associated with next state, which describes the controller and action
BoundAction nextBoundAction = nextState.BoundAction;
Console.WriteLine(nextBoundAction.Controller); // "Home"
Console.WriteLine(nextBoundAction.Action); // "Step3"

// Redirect to the action associated with the next state
RedirectToAction(nextBoundAction.Controller, nextBoundAction.Action);
```

**Given the user is not logged in, can they access "Step1"?**

```cs
ISzl stateMachine = BuildStateMachine(userIsLoggedIn: false);
BoundAction attemptedBoundAction = new BoundAction("Home", "Step1");
ISzl attemptedState = stateMachine.GetStateForAction(attemptedBoundAction);

// check if the attempted state is in the state machine
if (attemptedState == null)
{
    // Boot the user to `Login` if
    RedirectToAction("Home", "Login");
}
```
