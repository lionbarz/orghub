using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Actions;
using InterfaceAdapters.Models;

namespace InterfaceAdapters
{
    public class MeetingService
    {
        private readonly IDatabaseAccess _db;
        
        // TODO: HACK INSTEAD OF LOIGGINGIN IN 
        public static Guid _userId = Guid.NewGuid();
        
        public MeetingService(IDatabaseAccess db)
        {
            _db = db;
        }
        
        public async Task<Meeting> CreateMassMeetingAsync(Person host, DateTimeOffset start)
        {
            var meeting = Meeting.NewInstance(host, start, 0);
            await _db.AddMeetingAsync(meeting);
            return meeting;
        }

        public async Task<UXMeeting> AddMeeting(UXMeeting m)
        {
            var person = new Person() { Id = _userId, Name = "Mohamed" };
            var meeting = Meeting.NewInstance(person, DateTimeOffset.Now, 0);
            meeting.AddAttendee(new MeetingAttendee() { IsChair = true, IsMember = true, Person = person});
            await _db.AddMeetingAsync(meeting);
            return new UXMeeting();
        }
        
        public async Task<IEnumerable<UXMeeting>> GetMeetingsAsync()
        {
            var meetings = await _db.ListMeetingsAsync();
            return meetings.Select(x => new UXMeeting()
            {
                Id = x.Id,
                Chair = x.Chair.Name,
                State = x.GetMeetingState().GetDescription()
            });
        }

        public async Task TakeActionAsync(Guid meetingId, Guid personId, IAction action)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            // TODO: Return a status.
            meeting.Act(personId, action);
            await _db.UpdateMeetingAsync(meeting);
        }
    }
}