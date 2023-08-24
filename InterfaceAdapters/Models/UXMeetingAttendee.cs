using Core;

namespace InterfaceAdapters.Models
{
    public class UXMeetingAttendee
    {
        /// <summary>
        /// The person attending.
        /// </summary>
        public UxPerson Person { get; set; }
        
        /// <summary>
        /// The role of this attendee.
        /// </summary>
        public AttendeeRole Roles { get; set; }

        public static UXMeetingAttendee FromAttendee(MeetingAttendee attendee)
        {
            return new UXMeetingAttendee()
            {
                Person = PersonService.ToUxPerson(attendee.Person),
                Roles = attendee.Roles
            };
        }
    }
}