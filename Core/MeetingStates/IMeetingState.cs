using System.Collections.Generic;
using Core.Motions;

namespace Core.MeetingStates
{
    public interface IMeetingState
    {
        /// <summary>
        /// The type of this state. Ex: Adjourned, Voting.
        /// </summary>
        State Type { get; }
        
        /// <summary>
        /// Action to call the meeting to order.
        /// </summary>
        IMeetingState CallMeetingToOrder(MeetingAttendee actor);

        /// <summary>
        /// Declare (as the chair) that the time (voting, speaker) has expired.
        /// </summary>
        IMeetingState DeclareTimeExpired(MeetingAttendee actor);
        
        /// <summary>
        /// Action to move a main motion.
        /// </summary>
        IMeetingState MoveMainMotion(MeetingAttendee actor, IMainMotion motion);

        /// <summary>
        /// Action to move a subsidiary motion.
        /// </summary>
        IMeetingState MoveSubsidiaryMotion(MeetingAttendee actor, ISubsidiaryMotion motion);
        
        /// <summary>
        /// Action to move that the meeting adjourns until a certain date.
        /// </summary>
        IMeetingState MoveToAdjourn(MeetingAttendee actor);
        
        /// <summary>
        /// Action to second.
        /// </summary>
        IMeetingState Second(MeetingAttendee actor);
        
        /// <summary>
        /// Action to speak.
        /// </summary>
        IMeetingState Speak(MeetingAttendee actor);

        /// <summary>
        /// Cast a vote in a yes/not vote.
        /// </summary>
        IMeetingState Vote(MeetingAttendee actor, VoteType type);
        
        /// <summary>
        /// Action for a speaker to yield the floor back to whoever had it before.
        /// TODO: Ability to yield the remaining time to somebody else.
        /// </summary>
        IMeetingState Yield(MeetingAttendee actor);

        /// <summary>
        /// What actions can be taken during this state by this person.
        /// </summary>
        ICollection<ActionSupport> GetActionSupportForPerson(MeetingAttendee actor);

        /// <summary>
        /// What is going on at the meeting at this stage, in the present tense.
        /// Example: "The floor is open to speakers."
        /// </summary>
        string GetDescription();
    }
}