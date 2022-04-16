using System;
using System.Collections.Generic;
using Core.Actions;

namespace Core.MeetingStates
{
    public interface IMeetingState
    {
        /// <summary>
        /// Handle the action and return the next state, which
        /// could also be the current state if it's a loop.
        /// 
        /// If the action can't be handled, an exception is thrown.
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="action"></param>
        /// <returns>The new state to go to.</returns>
        IMeetingState TryHandleAction(MeetingAttendee actor, IAction action);

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