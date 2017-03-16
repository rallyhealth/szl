using System.Web.Routing;

namespace Szl.MvcDemo.Szl
{
    public class ProgramSzl : SzlBase
    {
        public ProgramSzl(int programId) : base(new []
            {
                new ActionableSzl("Program", "Step1", GetProgramParams(programId)),
                new ActionableSzl("Program", "Step2", GetProgramParams(programId)),
                new ActionableSzl("Program", "Step3", GetProgramParams(programId)),
            })
        {
        }

        private static RouteValueDictionary GetProgramParams(int programId)
        {
            return new RouteValueDictionary
            {
                {"programId", programId}
            };
        }
    }
}