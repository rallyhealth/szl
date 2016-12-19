using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Szl
{
    public interface ISzl
    {
        ISzl GetNextState(ISzl current);
        ISzl GetPreviousState(ISzl current);
        ISzl GetFurthestState();
        ISzl GetStateForAction(object action);
        ICollection<string> GetFlattenedRoutes();
    }

    public abstract class SzlBase : ISzl
    {
        private enum SearchDirection
        {
            Forward,
            Back
        }

        private readonly ICollection<SzlBase> _children;

        protected virtual bool IsActionableState { get { return false; } }

        protected SzlBase(ICollection<SzlBase> children)
        {
            _children = children ?? new SzlBase[0];
        }
        public ISzl GetNextState(ISzl current)
        {
            return DepthFirstSearch(current as SzlBase, SearchDirection.Forward, current == null);
        }

        public ISzl GetPreviousState(ISzl current)
        {
            if (current == null)
            {
                throw new ArgumentException("Current cannot be null");  
            } 
            return DepthFirstSearch(current as SzlBase, SearchDirection.Back, false);
        }

        private ISzl DepthFirstSearch(SzlBase currentState, SearchDirection direction, bool foundCurrentState)
        {
            if (foundCurrentState && IsActionableState)
            {
                return this; // we're done
            }
            if (currentState == null)
            {
                foundCurrentState = true;
            } 
            else if (currentState.Equals(this))
            {
                return currentState; // short circuit on the DFS per our convention that the next/prev actionable node of a state will either be a sibling or parent
            }

            var childrenWithGoodParenting = direction == SearchDirection.Forward ? _children : _children.AsEnumerable().Reverse(); // because they have direction #thxstlly
            foreach (var child in childrenWithGoodParenting)
            {
                var searchResult = child.DepthFirstSearch(currentState, direction, foundCurrentState);
                if (searchResult != null)
                {
                    if (searchResult.Equals(currentState))
                    {
                        if (IsActionableState)
                        {
                            return this;
                        }
                        foundCurrentState = true;
                        continue; // mark that we've found the current state but keep on keepin' on
                    }
                    return searchResult;
                }
            }

            return foundCurrentState ? currentState : null; // signal up if necessary that we've found the current state
        }

        public virtual ISzl GetFurthestState()
        {
            if (IsActionableState)
            {
                return this;
            }
            foreach (var child in _children.Reverse())
            {
                var furthestState = child.GetFurthestState();
                if (furthestState != null)
                {
                    return furthestState;
                }
            }
            return null;
        }

        public virtual ISzl GetStateForAction(object action)
        {
            foreach (var child in _children)
            {
                var result = child.GetStateForAction(action) as SzlBase;
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public virtual ICollection<string> GetFlattenedRoutes()
        {
            var routes = new List<string>();
            foreach (var child in _children)
            {
                routes.AddRange(child.GetFlattenedRoutes());
            }
            return routes;
        }

        protected virtual bool Equals(ISzl other)
        {
            return other != null && other.GetType() == GetType();
        }

        private Sizualization _sizualization
        {
            get { return GetSzlVisualization(parent: null); }
        }

        internal Sizualization GetSzlVisualization(Guid? parent)
        {
            var key = Guid.NewGuid();
            var type = GetType();
            var toString = ToString();
            return new Sizualization
            {
                // If ToString has been overridden, that's probably a better choice for display, otherwise, we want only the type's name
                Name = type.ToString() == ToString() ? GetType().Name : toString,
                Key = key,
                IsActionable = IsActionableState,
                Parent = parent,
                Children = _children.Select(a => a.GetSzlVisualization(key)).ToList()
            };
        }

        // `nq` gets rid of the double quoted nonsense
        [DebuggerDisplay("{_sizualizajson, nq}")]
        internal class Sizualization
        {
            public string Name { get; set; }
            public bool IsActionable { get; set; }
            public Guid? Parent { get; set; }
            public ICollection<Sizualization> Children { get; set; }
            public Guid Key { get; set; }

            private string _sizualizajson
            {
                get
                {
                    return JsonConvert.SerializeObject(
                        this,
                        new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()}
                        );
                }
            }
        }
    }
}
