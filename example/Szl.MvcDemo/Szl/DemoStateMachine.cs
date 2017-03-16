using System.Collections.Generic;

namespace Szl.MvcDemo.Szl
{
    public class DemoStateMachine : SzlBase
    {
        public DemoStateMachine(ICollection<SzlBase> children) : base(children) { }
    }
}