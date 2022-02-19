using System;
using System.Collections.Generic;
using System.Linq;
using Core.Actions;
using Core.MeetingStates;

namespace Core
{
    /// <summary>
    /// A group meeting, such as a regular meeting or mass meeting.
    /// This class doesn't know anything about the organization
    /// hosting it, or its membership, nor does it care. It just
    /// cares who can vote here and who is in charge here. It just
    /// runs the meeting.
    /// </summary>
    public class Meeting
    {
        /// <summary>
        /// Uniquely identifies this meeting.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// When the meeting will start.
        /// </summary>
        public DateTimeOffset StartTime { get; private init; }

        /// <summary>
        /// The attendees at any point of the meeting.
        /// </summary>
        public ICollection<MeetingAttendee> Attendees { get; private set; }
        
        /// <summary>
        /// The chair of the meeting. Usually the person who called the meeting.
        /// </summary>
        public Person Chair { get; private set; }

        /// <summary>
        /// How many members need to be present in order
        /// for business to be legally conducted.
        /// Zero means there is no minimum, or that the
        /// quorum is the number present at the time.
        /// </summary>
        public int Quorum { get; set; }

        /// <summary>
        /// Whether there is a quorum among the attendees.
        /// </summary>
        public bool HasQuorum()
        {
            var numCanVote = Attendees.Count(x => x.IsMember);
            return numCanVote >= Quorum;
        }
        
        /// <summary>
        /// The stack of meeting states. The last is the latest.
        /// It always starts with an adjourned state.
        /// </summary>
        private LinkedList<IMeetingState> MeetingStates { get; }

        private Meeting(Person chair, DateTimeOffset startTime, int quorum)
        {
            Id = Guid.NewGuid();
            StartTime = startTime;
            Attendees = new List<MeetingAttendee>();
            Chair = chair;
            Quorum = quorum;
            MeetingStates = new LinkedList<IMeetingState>();
            MeetingStates.AddLast(new AdjournedState());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bylaws"></param>
        /// <param name="chair">Who called the meeting</param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Meeting NewInstance(Person chair, DateTimeOffset startTime, int quorum)
        {
            return new Meeting(chair, startTime, quorum);
        }

        public void AddAttendee(MeetingAttendee attendee)
        {
            Attendees.Add(attendee);
        }

        public void RemoveAttendee(MeetingAttendee attendee)
        {
            Attendees.Remove(attendee);
        }

        public void Act(Guid personId, IAction action)
        {
            MeetingAttendee? actor = Attendees.FirstOrDefault(x => x.Person.Id == personId);

            if (actor == null)
            {
                throw new ArgumentException($"No person with ID {personId} is in the meeting.");
            }
            
            IMeetingState currentState = MeetingStates.Last();
            
            Console.WriteLine($"State: {currentState.GetDescription()}");
            Console.WriteLine($"Action: {action.Describe(actor.Person)}");

            if (action is MoveToAdjourn)
            {
                // Right now we immediately adjourn as soon as anyone suggests it.
                while (MeetingStates.Last() is not AdjournedState)
                {
                    MeetingStates.RemoveLast();
                }
                return;
            }
            
            if (currentState.TryHandleAction(actor, action, out IMeetingState? newState,
                out IAction? resultingAction))
            {
                if (newState != null)
                {
                    MeetingStates.AddLast(newState);
                    return;
                }
                
                // The current state is done. Go back to last state on stack.
                MeetingStates.RemoveLast();
                
                if (resultingAction != null)
                {
                    Act(actor.Person.Id, resultingAction);
                }
            }
            else
            {
                throw new ArgumentException(
                    $"{actor.Person.Name} can't take action {action} in meeting state {currentState}");
            }
        }

        public IMeetingState GetMeetingState()
        {
            return MeetingStates.Last();
        }
    }
}