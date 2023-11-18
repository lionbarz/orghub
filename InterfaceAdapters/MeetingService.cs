using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Meetings;
using Core.Motions;
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

        /// <summary>
        /// Add a new meeting for the group.
        /// </summary>
        /// <param name="personId">The person creating this meeting.</param>
        /// <param name="groupId"></param>
        /// <param name="startTime"></param>
        /// <param name="location"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<UXMeeting> CreateMeetingAsync(Guid personId, Guid groupId, DateTimeOffset startTime, string location, string description)
        {
            // TODO: Verify that the creator has permissions.
            var group = await _db.GetGroupAsync(groupId);
            var meeting = Meeting.NewInstance(
                group,
                startTime,
                description,
                location,
                MeetingAgenda.EmptyAgenda());
            await _db.AddMeetingAsync(meeting);
            return ToUxMeeting(meeting);
        }
        
        public async Task<UXMeeting> CreateMassMeetingAsync(Guid chairPersonId, DateTimeOffset start, string location, string description)
        {
            var person = await _db.GetPersonAsync(chairPersonId);
            var group = Group.NewInstance(person, "Temporary Group", "Create a permanent group");
            var meeting = Meeting.NewInstance(group, start, description, location, MeetingAgenda.EmptyAgenda());
            await _db.AddGroupAsync(group);
            await _db.AddMeetingAsync(meeting);
            return ToUxMeeting(meeting);
        }
        
        public async Task<IEnumerable<UXMeeting>> GetMeetingsAsync()
        {
            var meetings = await _db.ListMeetingsAsync();
            return meetings.Select(ToUxMeeting);
        }
        
        public async Task<IEnumerable<UXMeeting>> GetMeetingsForGroupAsync(Guid groupId)
        {
            // TODO: Optimize this so that it doesn't get all meetings from the database.
            var meetings = await _db.ListMeetingsAsync();
            var meetingForGroup = meetings.Where(x => x.GroupId == groupId);
            return meetingForGroup.Select(ToUxMeeting);
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
                Location = meeting.Location,
                State = meeting.State.GetDescription(),
                StateType = meeting.State.Type.ToString(),
                Attendees = meeting.Attendees.Select(UXMeetingAttendee.FromAttendee),
                HasQuorum = meeting.HasQuorum(),
                Agenda = meeting.Agenda.GetAllItems().Select(x => new UxAgendaItem()
                {
                    Title = x.GetTitle(),
                    IsCurrent = x.IsCurrent
                }),
                Minutes = meeting.Minutes.Select(m => new UXMinute()
                {
                    Text = m.Text,
                    Time = m.Time
                })
            };
        }
        
        public async Task MarkAttendance(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var person = await _db.GetPersonAsync(personId);
            meeting.AddAttendee(person);
            await _db.UpdateMeetingAsync(meeting);
        }
        
        public async Task<IEnumerable<string>> GetAvailableActions(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            var actionSupports = meeting.State.GetActionSupportForPerson(attendee);
            return actionSupports.Where(x => x.IsSupported).Select(x => x.Action.ToString());
        }
        
        public async Task CallToOrder(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            meeting.State.CallMeetingToOrder(attendee);
            await _db.UpdateMeetingAsync(meeting);
        }
        
        public async Task MoveResolution(Guid meetingId, Guid personId, string text)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var personRole = meeting.GetAttendee(personId);
            meeting.State.MoveMainMotion(personRole, new Resolve(text));
            await _db.UpdateMeetingAsync(meeting);
        }
        

        public async Task DeclareTimeExpired(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            meeting.State.DeclareTimeExpired(attendee);
            await _db.UpdateMeetingAsync(meeting);
        }
        
        public async Task Second(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            meeting.State.Second(attendee);
            await _db.UpdateMeetingAsync(meeting);
        }

        public async Task Speak(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            meeting.State.Speak(attendee);
            await _db.UpdateMeetingAsync(meeting);
        }
        
        public async Task Vote(Guid meetingId, Guid personId, VoteType voteType)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            meeting.State.Vote(attendee, voteType);
            await _db.UpdateMeetingAsync(meeting);
        }
        
        public async Task Yield(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            meeting.State.Yield(attendee);
            await _db.UpdateMeetingAsync(meeting);
        }

        public async Task MoveToAdjourn(Guid meetingId, Guid personId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            meeting.State.MoveToAdjourn(attendee);
            await _db.UpdateMeetingAsync(meeting);
        }
        
        public async Task NominateChair(Guid meetingId, Guid personId, Guid nomineePersonId)
        {
            var meeting = await _db.GetMeetingAsync(meetingId);
            var attendee = meeting.GetAttendee(personId);
            var nominee = await _db.GetPersonAsync(nomineePersonId);
            meeting.State.MoveMainMotion(attendee, new ElectChair(nominee));
            await _db.UpdateMeetingAsync(meeting);
        }
    }
}