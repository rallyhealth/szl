using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using NUnit.Framework;

namespace Szl.Tests
{
    [TestFixture]
    public class BoundActionTests
    {
        [Test]
        public void BoundAction_Matches_WhenRouteAndParametersMatch()
        {
            var actionParameters = new RouteValueDictionary
            {
                { "param1", "test" },
                { "param2", "test2" }
            };

            var otherActionParameters = new RouteValueDictionary
            {
                { "param1", "test" },
                { "param2", "test2" }
            };

            var boundAction = new BoundAction("testController", "action", actionParameters);
            var otherBoundAction = new BoundAction("testController", "action", otherActionParameters);

            Assert.That(boundAction.Matches(otherBoundAction), Is.True);
        }

        [Test]
        public void BoundAction_Matches_WhenRouteAndParametersMatch_And_CaseIsDifferent()
        {
            var actionParameters = new RouteValueDictionary
            {
                { "param1", "test" },
                { "param2", "test2" }
            };

            var otherActionParameters = new RouteValueDictionary
            {
                { "param1", "TEST" },
                { "PARAM2", "test2" }
            };

            var boundAction = new BoundAction("TESTCONTROLLER", "action", actionParameters);
            var otherBoundAction = new BoundAction("testController", "ACTION", otherActionParameters);

            Assert.That(boundAction.Matches(otherBoundAction), Is.True);
        }

        [Test]
        public void BoundAction_DoesNotMatch_WhenRouteMatchesButParametersDoNot()
        {
            var actionParameters = new RouteValueDictionary
            {
                { "param1", "test" },
                { "param2", "test2" }
            };

            var otherActionParameters = new RouteValueDictionary
            {
                { "param1", "test" },
            };

            var boundAction = new BoundAction("testController", "action", actionParameters);
            var otherBoundAction = new BoundAction("testController", "ACTION", otherActionParameters);

            Assert.That(boundAction.Matches(otherBoundAction), Is.False);
        }

        [Test]
        public void BoundAction_DoesNotMatch_WhenOtherBoundActionIsNull()
        {
            var actionParameters = new RouteValueDictionary
            {
                { "param1", "test" },
                { "param2", "test2" }
            };

            var boundAction = new BoundAction("testController", "action", actionParameters);

            Assert.That(boundAction.Matches(null), Is.False);
        }

        [Test]
        public void BoundAction_ProperlyFormats()
        {
            var actionParameters = new RouteValueDictionary
            {
                { "param1", "test" },
                { "param2", "test2" }
            };
            
            var boundAction = new BoundAction("testController", "action", actionParameters);

            Assert.That(boundAction.ToString(), Is.EqualTo("/testController/action?param1=test&param2=test2"));
        }

        [Test]
        public void DefaultConstructor_InitializesParameters()
        {
            // This is a silly test but I really wanted 100% coverage
            var boundAction = new BoundAction();

            Assert.That(boundAction.Params, Is.Not.Null);
        }
    }
}
