using System;
using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public interface IMeetingState
    {
        /// <summary>
        /// Tries to handle the action. If the action is handled,
        /// we either provide a new state that needs to be stacked
        /// on top of this one, or the result of this state which
        /// is passed to the previous state, or neither, which
        /// means that this state is done but has no return value.
        /// 
        /// If the action is not handled, it just means it's an
        /// invalid action that doesn't apply in this state and
        /// we're staying in this state.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="action"></param>
        /// <param name="newState">If not null, there's a new state
        /// the meeting goes to.</param>
        /// <param name="replaceCurrentState">If true, replace the current state with newState.</param>
        /// <param name="resultingAction"></param>
        /// <returns>True if it's handled.</returns>
        bool TryHandleAction(MeetingAttendee actor, IAction action, out IMeetingState? newState,
            out bool replaceCurrentState,
            out IAction? resultingAction);

        /// <summary>
        /// What actions can be taken during this state.
        /// </summary>
        IEnumerable<Type> GetSupportedActions(MeetingAttendee actor);

        /// <summary>
        /// The motions that can be moved during this state.
        /// </summary>
        IEnumerable<Type> GetSupportedMotions();

        /// <summary>
        /// What is going on at the meeting at this stage.
        /// </summary>
        string GetDescription();
    }
}