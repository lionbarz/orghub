using System;
using System.Collections.Generic;
using Core.Motions;

namespace Core.MeetingStates
{
    public interface IMeetingState
    {
        /// <summary>
        /// Action to call the meeting to order.
        /// </summary>
        IMeetingState CallMeetingToOrder(PersonRole actor);

        /// <summary>
        /// Declare (as the chair) that the time (voting, speaker) has expired.
        /// </summary>
        IMeetingState DeclareTimeExpired(PersonRole actor);
        
        /// <summary>
        /// Action to move a main motion.
        /// </summary>
        IMeetingState MoveMainMotion(PersonRole actor, IMainMotion motion);

        /// <summary>
        /// Action to move a subsidiary motion.
        /// </summary>
        IMeetingState MoveSubsidiaryMotion(PersonRole actor, ISubsidiaryMotion motion);
        
        /// <summary>
        /// Action to move that the meeting adjourns until a certain date.
        /// </summary>
        IMeetingState MoveToAdjourn(PersonRole actor);
        
        /// <summary>
        /// Action to second.
        /// </summary>
        IMeetingState Second(PersonRole actor);
        
        /// <summary>
        /// Action to speak.
        /// </summary>
        IMeetingState Speak(PersonRole actor);

        /// <summary>
        /// Cast a vote in a yes/not vote.
        /// </summary>
        IMeetingState Vote(PersonRole actor, VoteType type);
        
        /// <summary>
        /// Action for a speaker to yield the floor back to whoever had it before.
        /// TODO: Ability to yield the remaining time to somebody else.
        /// </summary>
        IMeetingState Yield(PersonRole actor);

        /// <summary>
        /// What actions can be taken during this state by this person.
        /// </summary>
        ICollection<ActionSupport> GetActionSupportForPerson(PersonRole actor);

        /// <summary>
        /// What is going on at the meeting at this stage, in the present tense.
        /// Example: "The floor is open to speakers."
        /// </summary>
        string GetDescription();
    }
}