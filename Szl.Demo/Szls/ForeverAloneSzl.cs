using System.Collections.Generic;
using System.Web.Routing;

namespace Szl.Demo.Szls
{
    public class ForeverAloneActionableSzl : ActionableSzl
    {
        public ForeverAloneActionableSzl() : base("Forever", "Alone", null) { }

        public override ISzl GetFurthestState()
        {
            return this;
        }
    }

    public class HighlanderActionableSzl : ActionableSzl
    {
        public HighlanderActionableSzl() : base("One", "There Can Be Only", null) { }

        public override ISzl GetFurthestState()
        {
            return this;
        }
    }

    public class ForeverACloneActionableSzl : ActionableSzl
    {
        public ForeverACloneActionableSzl(int platoonId, int dogTagId)
            : base("Forever", "Clone", new RouteValueDictionary { { "Platoon: ", platoonId }, { "DogTag: ", dogTagId } })
        {
        }

        public override ISzl GetFurthestState()
        {
            return this;
        }
    }

    public class GeneralGrievousSzl : SzlBase
    {
        public GeneralGrievousSzl(ICollection<SzlBase> children) : base(children) { }
    }
}
