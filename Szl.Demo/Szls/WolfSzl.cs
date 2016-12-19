using System.Collections.Generic;

namespace Szl.Demo.Szls
{
    public class WolfPackSzl : SzlBase
    {
        public WolfPackSzl() : base(new List<SzlBase> 
            {
                new AlphaWolfActionableSzl(),
                new BlueWolfActionableSzl(),
                new CharlieWolfActionableSzl(),
                new DeltaWolfActionableSzl(),
                new OmegaWolfActionableSzl()
            })
        { }
    }

    public class WolfActionableSzl : ActionableSzl
    {
        private readonly string _action;
        public WolfActionableSzl(string action) : base("Wolf", action)
        {
            _action = action;
        }

        public override ISzl GetFurthestState()
        {
            return this;
        }
    }

    public class AlphaWolfActionableSzl : WolfActionableSzl
    {
        public AlphaWolfActionableSzl() : base("Alpha") { }
    }

    public class BlueWolfActionableSzl : WolfActionableSzl
    {
        public BlueWolfActionableSzl() : base("Blue") { }
    }

    public class CharlieWolfActionableSzl : WolfActionableSzl
    {
        public CharlieWolfActionableSzl() : base("Charlie") { }
    }

    public class DeltaWolfActionableSzl : WolfActionableSzl
    {
        public DeltaWolfActionableSzl() : base("Delta") { }
    }

    public class OmegaWolfActionableSzl : WolfActionableSzl
    {
        public OmegaWolfActionableSzl() : base("Omega") { }
    }
}
