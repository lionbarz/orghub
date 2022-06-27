using System;
using Action = Core.Actions.Action;

namespace Core
{
    /// <summary>
    /// An action taken by a person takes one state to another.
    /// </summary>
    public class StateTransition : Tuple<Action, Person>
    {
        /// <summary>
        /// The action that is taken.
        /// </summary>
        public Action Action => Item1;

        /// <summary>
        /// The person that is taking it.
        /// </summary>
        public Person Person => Item2;

        public static StateTransition InstanceOf(Action action, Person person)
        {
            return new StateTransition(action, person);
        }

        private StateTransition(Action action, Person person) : base(action, person) { }
    }
}