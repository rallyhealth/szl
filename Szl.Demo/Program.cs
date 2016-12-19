using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Szl.Demo.Szls;

namespace Szl.Demo
{
    public class Program
    {
        private static DemoStateMachine _machine;
        private static DemoContext _context;
        private static ISzl _current;
        private static List<SzlBase> _children;
        static void Main()
        {
            _context = new DemoContext();
            _children = new List<SzlBase>();
            _current = null;
            Intro();
            _machine = new DemoStateMachine(_children);
            Florence();
        }

        private static void Florence()
        {
            Console.WriteLine("Let's do this. You can type NEXT or BACK to navigate through the machine.");
            Console.WriteLine("UNLEASH or LEASH to handle the KRAKEN lurking within");
            Console.WriteLine("JUMP to do some teleportation");
            Console.WriteLine("EXIT to go home");
            Console.WriteLine("We'll do it live!");
            Console.WriteLine();
            string input;
            do
            {
                input = Console.ReadLine().ToLower();
                switch (input)
                {
                    case "next":
                        _current = _machine.GetNextState(_current);
                        PrintState(_current as ActionableSzl);
                        break;
                    case "back":
                        _current = _machine.GetPreviousState(_current);
                        PrintState(_current as ActionableSzl);
                        break;
                    case "unleash":
                        _context.UnleashTheKraken = true;
                        Console.WriteLine("Who let the dogs out?");
                        Console.WriteLine();
                        break;
                    case "leash":
                        _context.UnleashTheKraken = false;
                        Console.WriteLine("Containment protocol initiated");
                        Console.WriteLine();
                        break;
                    case "jump":
                        Jump();
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Not sure what you meant. Try again.");
                        Console.WriteLine();
                        break;
                }
            } while (input != "exit");
            Console.WriteLine("K Bye");
        }

        private static void PrintState(ActionableSzl state)
        {
            if (state != null)
            {
                var action = state.BoundAction;
                Console.WriteLine(action.Controller);
                Console.WriteLine(action.Action);
                if (action.Params != null && action.Params.Count > 0)
                {
                    foreach (var param in action.Params)
                    {
                        Console.WriteLine(param.Key + ": " + param.Value);
                    }
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Slow your roll, buddy. Back at start.");
                Console.WriteLine();
            }
        }

        private static void Intro()
        {
            Console.WriteLine("Szl Choose Your Own Adventure Initiated");
            Console.WriteLine();
            Thread.Sleep(1000);
            Console.WriteLine("You wake up. It's a dark room. What do you do?");
            Console.WriteLine();
            Thread.Sleep(1000);
            Console.WriteLine("Just kidding. We'll be a little more straightforward. Here's the scoop:");
            Console.Write("We'll configure the state machine, you'll tell me when you're ready to go,");
            Console.WriteLine("and then you'll be free to navigate forward and back as you please.");
            Console.WriteLine("You can even change the context of the app at any time.");
            Console.WriteLine();
            Thread.Sleep(1000);
            Console.WriteLine("OK. Let's build that state machine. Options for states to add are:");
            Console.WriteLine("HIGHLANDER, ALONE, CLONES, WOLFPACK, and KRAKEN");
            Console.WriteLine("Only CLONES can be used more than once.");
            Console.WriteLine("Or you can type QUICK to build a quick version of this demo");
            Console.WriteLine("Type EXIT to finish building the state machine.");
            Console.WriteLine();
            string input;
            do
            {
                input = Console.ReadLine().ToLower();
                switch (input)
                {
                    case "highlander":
                        AddHighlander();
                        break;
                    case "alone":
                        AddForeverAlone();
                        break;
                    case "clones":
                        AddClones();
                        break;
                    case "wolfpack":
                        AddWolfPack();
                        break;
                    case "kraken":
                        AddKraken();
                        break;
                    case "quick":
                        AddForeverAlone();
                        AddWolfPack();
                        AddKraken();
                        AddClones(2);
                        AddHighlander();
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Not sure what you meant. Try again.");
                        Console.WriteLine();
                        break;
                }
            } while (input != "exit" && input != "quick");

            Console.WriteLine("Good stuff! Config Time:");
            Console.WriteLine("Unleash the kraken? (Y/N)");
            input = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (input == "y")
            {
                _context.UnleashTheKraken = true;
            }

            Console.WriteLine("Alrighty. Time to start this journey!");
            Console.WriteLine();
        }

        private static void Jump()
        {
            Console.WriteLine("Okay Mr. Fancy Footwork. Let's jump to a state.");
            Console.WriteLine("Type ALONE, HIGHLANDER, or KRAKEN to go to one of those states.");
            Console.WriteLine("Type {CALLSIGN}WOLF (ALPHA, BLUE, CHARLIE, DELTA, or OMEGA)");
            Console.WriteLine("to go to one of those states. Or, type CLONE{PLATOON}X{DOGTAG#}");
            Console.WriteLine("to jump to a clone state.");
            _context.SkipToThisState = Console.ReadLine().ToLower();
            Console.WriteLine();
            _current = _machine.GetFurthestState();
            PrintState(_current as ActionableSzl);
        }

        private static void AddForeverAlone()
        {
            if (_children.Any(s => s is ForeverAloneActionableSzl))
            {
                Console.WriteLine("Stop it");
                Console.WriteLine();
                return;
            }
            _children.Add(new ForeverAloneActionableSzl());
            Console.WriteLine("A wolf pack of one");
            Console.WriteLine();
        }

        private static void AddHighlander()
        {
            if (_children.Any(s => s is HighlanderActionableSzl))
            {
                Console.WriteLine("Stop it");
                Console.WriteLine();
                return;
            }
            _children.Add(new HighlanderActionableSzl());
            Console.WriteLine("There can be only one");
            Console.WriteLine();
        }

        private static void AddWolfPack()
        {
            if (_children.Any(s => s is WolfPackSzl))
            {
                Console.WriteLine("Stop it");
                Console.WriteLine();
                return;
            }
            _children.Add(new WolfPackSzl());
            Console.WriteLine("It's a full moon out there");
            Console.WriteLine();
        }

        private static void AddClones(int num = -1)
        {
            var platoonId = 0;
            if (num == -1)
            {
                Console.WriteLine("The Clone Wars have started. How many do you fight off?");
                Console.WriteLine("(Gimme a non-negative integer)");
                var input = Console.ReadLine();
                Console.WriteLine();
                var success = int.TryParse(input, out num);
                if (!success || num < 0)
                {
                    num = 0;
                }
                Console.Write("What's the platoon number for these troops? ");
                input = Console.ReadLine();
                Console.WriteLine();
                success = int.TryParse(input, out platoonId);
                if (!success || num < 0)
                {
                    platoonId = 0;
                }
            }
            var clones = new List<SzlBase>();
            for (var i = 0; i < num; i++)
            {
                clones.Add(new ForeverACloneActionableSzl(platoonId, i));
            }
            _children.Add(new GeneralGrievousSzl(clones));
        }

        private static void AddKraken()
        {
            if (_children.Any(s => s is KrakenActionableSzl))
            {
                Console.WriteLine("Stop it");
                Console.WriteLine();
                return;
            }
            _children.Add(new KrakenActionableSzl());
            Console.WriteLine("Woah there, Zeus. Be careful that you do not come to regret what you've wrought");
            Console.WriteLine();
        }
    }
}
