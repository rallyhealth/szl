using Szl.MvcDemo.Szl;
using NUnit.Framework;

namespace Szl.MvcDemo.Tests
{
    [TestFixture]
    public class DemoStateMachineTests
    {
        [Test]
        public void GetWizardSzls_WhenAllowBlockedNodeIsFalse_ContainsStep1()
        {
            var allowBlockedNode = false;
            
            var wizardSzls = StateMachineBuilder.GetWizardSzls(allowBlockedNode);
            var stateMachine = new DemoStateMachine(wizardSzls);

            var program1Action = new BoundAction("Wizard", "Step1");
            var matchingState = stateMachine.GetStateForAction(program1Action);

            Assert.That(matchingState, Is.Not.Null);
        }

        [Test]
        public void GetWizardSzls_WhenAllowBlockedNodeIsFalse_DoesNotContainStep3()
        {
            var allowBlockedNode = false;

            var wizardSzls = StateMachineBuilder.GetWizardSzls(allowBlockedNode);
            var stateMachine = new DemoStateMachine(wizardSzls);

            var program3Action = new BoundAction("Wizard", "Step3");
            var matchingState = stateMachine.GetStateForAction(program3Action);

            Assert.That(matchingState, Is.Null);
        }

        [Test]
        public void GetWizardSzls_WhenAllowBlockedNodeIsTrue_DoesContainStep3()
        {
            var allowBlockedNode = true;

            var wizardSzls = StateMachineBuilder.GetWizardSzls(allowBlockedNode);
            var stateMachine = new DemoStateMachine(wizardSzls);

            var program3Action = new BoundAction("Wizard", "Step3");
            var matchingState = stateMachine.GetStateForAction(program3Action);

            Assert.That(matchingState, Is.Not.Null);
        }

        [Test]
        public void GetWizardSzls_WhenAllowBlockedNodeIsFalse_NextFrom1Is2()
        {
            var wizardSzls = StateMachineBuilder.GetWizardSzls(allowBlockedNode : false);
            var stateMachine = new DemoStateMachine(wizardSzls);
            var step1BoundAction = new BoundAction("Wizard", "Step1");
            var step1State = stateMachine.GetStateForAction(step1BoundAction);

            var nextState = stateMachine.GetNextState(step1State);
            var nextStateBoundAction = ((ActionableSzl) nextState).BoundAction;

            Assert.That(nextStateBoundAction.Action, Is.EqualTo("Step2"));
        }

        [Test]
        public void GetWizardSzls_WhenAllowBlockedNodeIsFalse_NextFrom2IsNotStep3()
        {
            var wizardSzls = StateMachineBuilder.GetWizardSzls(allowBlockedNode: false);
            var stateMachine = new DemoStateMachine(wizardSzls);
            var step2BoundAction = new BoundAction("Wizard", "Step2");
            var step2State = stateMachine.GetStateForAction(step2BoundAction);

            var nextState = stateMachine.GetNextState(step2State);
            var nextStateBoundAction = ((ActionableSzl)nextState).BoundAction;

            Assert.That(nextStateBoundAction.Action, Is.Not.EqualTo("Step3"));
        }

        [Test]
        public void GetWizardSzls_WhenAllowBlockedNodeIsTrue_NextFrom2IsStep3()
        {
            var wizardSzls = StateMachineBuilder.GetWizardSzls(allowBlockedNode: true);
            var stateMachine = new DemoStateMachine(wizardSzls);
            var step2BoundAction = new BoundAction("Wizard", "Step2");
            var step2State = stateMachine.GetStateForAction(step2BoundAction);

            var nextState = stateMachine.GetNextState(step2State);
            var nextStateBoundAction = ((ActionableSzl)nextState).BoundAction;

            Assert.That(nextStateBoundAction.Action, Is.EqualTo("Step3"));
        }
    }
}
