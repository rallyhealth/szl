# README #

This repo contains our attempt to fix Choice's routing by writing a standalone routing framework called Szl. (aka Routing v3)

It is a state-machine-of-state-machines concept, and lives here in generic form. To use it in your project, take a look at either of our sample projects -- a console app and an MVC app. 

A state machine implemented with Szl exists as a single Szl made up of any number of child Szls. We recommend using factory methods to build out the exact structure. These factories should contain methods that are pure functions of app state. This allows your app to utilize any context it needs to add or remove states as necessary. Your app can then consult this containing Szl for any routing/navigation needs. The expected usage is to rebuild the state machine every time your app's state or context changes so that the machine only contains states that are currently valid. The recommended pattern is to have a single general Szl factory composed of smaller factories that build out the pieces of your state machine.

All Szls can have children, allowing for an arbitrary depth to the state machine. When searching for a state, however, the machine will only stop on actionable states, denoted by the IsActionableState property. By default, the base class is not actionable. A child ActionableSzl class is provided from which actionable states can be implemented. 

It is important to note that while the state machine generally navigates the state machine using depth-first search, a distinction is drawn for the children of actionable nodes. We have adopted a convention that allows an actionable node with children to behave in a hub-and-spokes manner. This means that while the children of a non-actionable node are iterated linearly, the direct children of an actionable node exist as extensions of the hub. They are valid states and can be linked to, but will not be traversed as a "next" state of the machine from the hub. However, the "next" state of a spoke will always be the hub. 

By combining both non-actionable container Szls and actionable hub Szls, most desired route maps can be constructed. For example, if it were desired to have Szls traversed linearly from a hub, they could be contained as children of a non-actionable Szl that was a child of the hub.

Actionable Szls by convention have both a bound action and an action identifier. This allows your app to map Szl states with pages or methods in your application. In the case of ActionableSzl, which is intended as an MVC-friendly version of a Szl, these are tightly coupled as the bound action is the controller name, action name, and params dictionary, and the identifier is the URL constructed from that information. ReactSzl, by contrast, is intended to be used as a way to inject a SPA into Choice. In practice, the bound action will always map to the the MVC method that loads up the SPA, but the action identifier will provide logical state to allow the SPA to understand its context.

Szl exposes the following interface: GetNextState, GetPreviousState, GetFurthestState, GetStateForAction, and GetFlattenedRoutes.

Both GetNextState and GetPreviousState will retrieve the appropriate state from the map given the current Szl passed in as a parameter. These are intended to be called only on the parent Szl that contains the entire state machine. The current state can be retrieved by using GetStateForAction. This should be passed an object signifying the current action and is the method to use for checking if a state exists in the current map. ActionableSzls expect a BoundAction and will compare their BoundAction to that which was passed in. ReactSzls expect an action identifier and will compare theirs to the one passed in. 

GetFurthestState is useful when loading up the app to retrieve the state furthest in the machine. Due to our assumption that the machine only contains valid states, this allows up to simply return the last child of the last child of the machine (recursively taken as needed). 

GetFlattenedRoutes provides a list of all the action identifiers that exist in the state machine. This is intended to provide context to a SPA using Szl for its routing needs.

GetNextState and GetPreviousState have fixed behavior and are not virtual. However, GetFurthestState, GetStateForAction, and GetFlattenedRoutes are virtual and are intended to be the avenues by which child classes inject any behavioral differences they require. An example of this would be a containing Szl overriding GetFurthestState and/or GetStateForAction to always return its first child even if its other children are valid states. In this way, it can control expected routing and enforce any desired UX. 