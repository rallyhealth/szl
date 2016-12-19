using System.Collections.Generic;
using System.Linq;

namespace Szl.MvcDemo.Szl
{
    public static class StateMachineBuilder
    {
        public static DemoStateMachine GetStateMachine(EnrollmentState enrollmentState)
        {
            var states = new List<SzlBase>();
            states.AddRange(GetWizardSzls(enrollmentState.AllowBlockedNode));
            states.AddRange(GetProgramSzls(enrollmentState.NumberOfPrograms));
            return new DemoStateMachine(states);
        }

        public static ICollection<SzlBase> GetWizardSzls(bool allowBlockedNode)
        {
            var steps = new List<SzlBase>
            {
                new ActionableSzl("Wizard", "Step1"),
                new ActionableSzl("Wizard", "Step2"),
            };
            if (allowBlockedNode)
            {
                steps.Add(new ActionableSzl("Wizard", "Step3")); 
            }
            return steps;
        }

        public static ICollection<ProgramSzl> GetProgramSzls(int count)
        {
            return Enumerable.Range(0, count).Select(i => new ProgramSzl(i)).ToList();
        }
    }
}