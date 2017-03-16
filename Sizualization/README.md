# Sizualization

Debugging Szl can be a bit tricky since the only interface exposed of the state machine is the root Szl itself. Because the `ActionableSzl`s you're looking for might be nested a dozen levels deep, this can lead to frustrating drill-down in the Visual Studio debugger.

To alleviate these pains, I introduce Sizualiztion: A comprehensive and interactive tree view of a Szl state machine.

## Viewing the Sizualization

This repo contains a sample Szl dump from a test enrollment in Choice. View it by following these steps.

1. Get Python (`cinst python2 -y` if you don't have it)
2. `cd` to the `Sizualization` directory (it should contain `data.json` and `index.html`)
3. Run `python -m SimpleHTTPServer`
4. In a browser, navigate to http://localhost:8000/

Congrats! You can now open and close nodes to your heart's desire. `ActionableSzl`s will be labeled with their associated `BoundAction`s and all other container Szls are labeled with their class name (`CensusSzl`, `ProgramSzl`, etc)

## Viewing your own Sizualization from Choice

To visualize a snapshot of the Szl that you're interested in, the steps are the same as above, but we need to use the JSON representation of the current Szl state. To get that:

1. Get your enrolling Choice user in the right state that you want to test
2. Open Titanis/Spotlite.Web in Visual Studio
3. Attach the Debugger
4. In the Spotlite.Web `BaseController`, set a breakpoint at a part of the request lifecycle that you want to test. This is typically in `MoveSzl` or `OnActionExecuting`
5. To view the JSON string of the Szl, you have two options
  - If there's a reference to the Szl state machine in scope, mouse over the variable, drill down once, then right click and "Copy Value" of the `_sizualization` property
  - If there's not a Szl state machine in scope, open the "Watch" window and add a watch for `GetStateMachine()`, drill down in to the result, then right click and "Copy Value" of the `_sizualization` property
6. Paste the JSON as-is into the `data.json` file within the `Sizualization` directory.
7. Follow the steps in "Viewing the Sizualization" above to start the web server or refresh the page if it's running to see the updated tree
