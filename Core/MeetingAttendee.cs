namespace Core
{
    public class MeetingAttendee
    {
        /// <summary>
        /// Who the person is.
        /// </summary>
        public Person Person { get; set; }
        
        /// <summary>
        /// Is this a voting member?
        /// </summary>
        public bool IsMember { get; set; }
        
        /// <summary>
        /// Whether this person chairing the meeting.
        /// </summary>
        public bool IsChair { get; set; }
    }
}