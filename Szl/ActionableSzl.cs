using System.Collections.Generic;
using System.Web.Routing;

namespace Szl
{
    public interface IActionableSzl : ISzl
    {
        BoundAction BoundAction { get; }
        string GetRouteUrl();
    }

    public class ActionableSzl : SzlBase, IActionableSzl
    {
        private readonly BoundAction _boundAction;
        protected override bool IsActionableState { get { return true; } }

        public BoundAction BoundAction
        {
            get { return _boundAction; }
        }

        public ActionableSzl(string boundController, string boundAction, RouteValueDictionary boundParams = null, ICollection<SzlBase> children = null)
            : base(children)
        {
            _boundAction = new BoundAction
            {
                Controller = boundController,
                Action = boundAction,
                Params = boundParams ?? new RouteValueDictionary()
            };
        }

        public virtual string GetRouteUrl()
        {
            return BoundAction.ToString();
        }

        public override ISzl GetStateForAction(object action)
        {
            var state = base.GetStateForAction(action);
            if (state != null)
            {
                return state;
            }

            if (BoundAction.Matches(action as BoundAction))
            {
                return this;
            }
            return null;
        }

        public override ICollection<string> GetFlattenedRoutes()
        {
            var routes = new List<string>();
            routes.Add(GetRouteUrl());
            routes.AddRange(base.GetFlattenedRoutes());
            return routes;
        }

        protected override bool Equals(ISzl other)
        {
            var otherMvc = other as ActionableSzl;
            if (otherMvc == null)
            {
                return false;
            }
            return BoundAction.Matches(otherMvc.BoundAction);
        }

        public override string ToString()
        {
            return BoundAction.ToString();
        }
    }
}
