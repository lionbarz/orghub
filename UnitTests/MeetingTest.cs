using System;
using Core;
using Core.Actions;
using Core.MeetingStates;
using Core.Motions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MeetingTest
    {
        [TestMethod]
        public void NoQuorum()
        {
            // Stage
            var mo = new Person();
            
            // Act
            var meetingTime = (new DateTimeOffset(DateTime.Now)) + TimeSpan.FromDays(7);
            var meeting = Meeting.NewInstance(mo, meetingTime, 2);
            meeting.AddAttendee(new MeetingAttendee()
            {
                Person = mo,
                IsMember = true
            });
            
            // Verify
            Assert.AreEqual(false, meeting.HasQuorum());
        }
        
        [TestMethod]
        public void HasQuorum()
        {
            // Stage
            var mo = new Person();
            
            // Act
            var meetingTime = (new DateTimeOffset(DateTime.Now)) + TimeSpan.FromDays(7);
            var meeting = Meeting.NewInstance(mo, meetingTime, 0);
            meeting.AddAttendee(new MeetingAttendee()
            {
                Person = mo,
                IsMember = true
            });
            
            // Verify
            Assert.AreEqual(true, meeting.HasQuorum());
        }

        [TestMethod]
        public void StartSpeakMoveAdjourn()
        {
            // Stage
            var mo = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Mo"
            };
            var roni = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Roni"
            };
            var moAttendee = new MeetingAttendee()
            {
                Person = mo,
                IsMember = true,
                IsChair = true
            };
            var roniAttendee = new MeetingAttendee()
            {
                Person = roni,
                IsMember = true
            };
            var meetingTime = (new DateTimeOffset(DateTime.Now)) + TimeSpan.FromDays(7);
            var meeting = Meeting.NewInstance(mo, meetingTime, 0);
            meeting.AddAttendee(moAttendee);
            meeting.AddAttendee(roniAttendee);
            
            // Act
            meeting.Act(moAttendee, new CallMeetingToOrder());
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(OpenFloorState));
            meeting.Act(roniAttendee, new Speak());
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(SpeakerHasFloorState));
            meeting.Act(moAttendee, new ExpireSpeakerTime());
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(OpenFloorState));
            meeting.Act(roniAttendee, new Move(new Resolve(roni, "Go exploring")));
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(DebateState));
            meeting.Act(moAttendee, new MoveToAdjourn());
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(AdjournedState));

            // Verify
        }
        
        [TestMethod]
        public void StartElectChairAdjourn()
        {
            // Stage
            var mo = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Mo"
            };
            var roni = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Roni"
            };
            var group = Group.WithMembership(new []{mo, roni});
            var moAttendee = new MeetingAttendee()
            {
                Person = mo,
                IsMember = true,
                IsChair = true
            };
            var roniAttendee = new MeetingAttendee()
            {
                Person = roni,
                IsMember = true
            };
            var meetingTime = (new DateTimeOffset(DateTime.Now)) + TimeSpan.FromDays(7);
            var meeting = Meeting.NewInstance(mo, meetingTime, 0);
            meeting.AddAttendee(moAttendee);
            meeting.AddAttendee(roniAttendee);
            
            // Act
            /*
            meeting.Act(mo.Id, new CallMeetingToOrder());
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(OpenFloorState));
            
            meeting.Act(mo.Id, new Move(new ElectChair(mo, group, mo)));
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(DebateState));
            
            meeting.Act(mo.Id, new DeclareMotionPassed());
            Assert.AreEqual(mo, group.Chair);
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(OpenFloorState));

            meeting.Act(mo.Id, new MoveToAdjourn());
            Assert.IsInstanceOfType(meeting.GetMeetingState(), typeof(AdjournedState));
*/
            // Verify
        }
    }
}