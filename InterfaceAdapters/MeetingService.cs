using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core;
using InterfaceAdapters.Models;

namespace InterfaceAdapters
{
    public class MeetingService
    {
        private readonly IDatabaseAccess _db;

        public MeetingService(IDatabaseAccess db)
        {
            _db = db;
        }
        
        public async Task<UXMeeting> CreateMassMeetingAsync(Guid chairPersonId, DateTimeOffset start, string location, string description)
        {
            var person = await _db.GetPersonAsync(chairPersonId);
            var group = Group.NewInstance(person);
            var meeting = Meeting.NewInstance(start, description, location, 0);
            await _db.AddGroupAsync(group);
            await _db.AddMeetingAsync(meeting);
            return ToUxMeeting(meeting);
        }
        
        public async Task<IEnumerable<UXMeeting>> GetMeetingsAsync()
        {
            var meetings = await _db.ListMeetingsAsync();
            return meetings.Select(ToUxMeeting);
        }

        public async Task<UXMeeting> GetMeetingAsync(Guid meetingId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            return ToUxMeeting(meeting);
        }

        public static UXMeeting ToUxMeeting(Meeting meeting)
        {
            return new UXMeeting()
            {
                Id = meeting.Id,
                Description = meeting.Description,
                StartTime = meeting.StartTime.ToString("o", CultureInfo.InvariantCulture),
                Location = meeting.Location
            };
        }
    }
}