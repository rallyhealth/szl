# Szl.MvcDemo.Tests

An example of testing the Szl configurations for an Mvc application.

These tests demonstrate that modules of Szl configuration can be tested independently of the root state machine. The "wizard" section is tested in isolation from the "program" section. Tests are based on searching the state machine for given states and asserting their existence or absence.