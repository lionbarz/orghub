using System;
using System.Collections.Generic;
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
        
        public async Task<UXMeeting> CreateMassMeetingAsync(Guid chairPersonId, DateTimeOffset start)
        {
            var person = await _db.GetPersonAsync(chairPersonId);
            var group = Group.NewInstance(person);
            var meeting = Meeting.NewInstance(group.Id, group, person, start, 0);
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

        public async Task CallToOrder(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var groupId = meeting.GroupId;
            var group = await _db.GetGroupAsync(groupId);
            var person = await _db.GetPersonAsync(personId);
            var personRole = group.CreatePersonRole(person);
            meeting.State.CallMeetingToOrder(personRole);
            await _db.UpdateMeetingAsync(meeting);
        }

        private UXMeeting ToUxMeeting(Meeting meeting)
        {
            return new UXMeeting()
            {
                Id = meeting.Id,
                Chair = meeting.Chair.Name,
                State = meeting.State.GetDescription()
            };
        }
    }
}