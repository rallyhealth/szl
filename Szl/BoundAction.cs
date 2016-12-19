using System;
using System.Linq;
using System.Web.Routing;

namespace Szl
{
    public class BoundAction
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public RouteValueDictionary Params { get; set; }

        public BoundAction()
        {
            Params = new RouteValueDictionary();
        }

        public BoundAction(string controller, string action, RouteValueDictionary @params = null)
        {
            Controller = controller;
            Action = action;
            Params = @params ?? new RouteValueDictionary();
        }

        public bool Matches(BoundAction other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Controller.Equals(Controller, StringComparison.InvariantCultureIgnoreCase) 
                && other.Action.Equals(Action, StringComparison.InvariantCultureIgnoreCase) 
                && ParamsMatch(other.Params);
        }

        public override string ToString()
        {
            var urlParams = Params.Any() 
                ? "?" + string.Join("&", Params.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)))
                : string.Empty;
            return string.Format("/{0}/{1}{2}", Controller, Action, urlParams);
        }

        private bool ParamsMatch(RouteValueDictionary otherParams)
        {
            foreach (var param in Params)
            {
                if (!otherParams.ContainsKey(param.Key) ||
                    !param.Value.ToString().Equals(otherParams[param.Key].ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
