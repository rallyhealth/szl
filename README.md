# Szl

Szl is a C# library for creating routing state machines. It includes a reference implementation for use in ASP.NET MVC-style applications, but the core library is adaptable for other potential implementations.

A Szl configuration answers the following questions:

- Given my context (am I logged in, have I answered a required question, etc.), can I go to X (where x is an application state or page)
- Given my context and that I am on State X, what state is Next?
- Given my context and that I am on State X, what state is Back?

A Szl configuration is a pure function (data in, data out, no side effects) and is highly testable because they are composite in nature and can be nested.

## Getting Started

A reference Szl state machine implementation is provided in "Szl.MvcDemo" and "Szl.MvcDemo.Tests".

More detailed setup instructions to come.
