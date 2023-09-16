using System;
using System.Collections.Generic;
using Core;
using Core.Meetings;
using Core.Motions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MeetingTest
    {
        [TestMethod]
        public void ElectChair()
        {
            var mo = new Person("Mo", "mo@gmail.com");
            var omar = new Person("Omar", "omar@gmail.com");
            var group = Group.NewInstance(mo, "Lebanese-American Union", "Reform Lebanon", new List<Person>() { omar });
            group.AddMember(omar);
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online", MeetingAgenda.EmptyAgenda());
            var moAttendee = meeting.AddAttendee(mo);
            var omarAttendee = meeting.AddAttendee(omar);
            
            meeting.State.CallMeetingToOrder(moAttendee);
            meeting.State.MoveMainMotion(omarAttendee, new ElectChair(omar));
            meeting.State.Second(moAttendee);
            
            // Moves to a vote.
            meeting.State.DeclareTimeExpired(moAttendee);

            meeting.State.Vote(moAttendee, VoteType.Aye);
            meeting.State.Vote(omarAttendee, VoteType.Aye);
            meeting.State.DeclareTimeExpired(moAttendee);
            
            Assert.AreEqual(omar, group.Chair);
        }

        [TestMethod]
        public void TestHasQuorum()
        {
            var mo = new Person("Mo", "mo@gmail.com");
            var omar = new Person("Omar", "omar@gmail.com");
            var group = Group.NewInstance(mo, "Lebanese-Americans For Change", "Reform Lebanon", new List<Person>() { omar });
            group.AddMember(omar);
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online", MeetingAgenda.EmptyAgenda());
            meeting.AddAttendee(mo);
            meeting.AddAttendee(omar);
            Assert.IsTrue(meeting.HasQuorum());
        }
        
        /// <summary>
        /// There's just the chair and nobody else. It should have quorum.
        /// </summary>
        [TestMethod]
        public void TestHasQuorumWithChair()
        {
            var mo = new Person("Mo", "mo@gmail.com");
            var group = Group.NewInstance(mo, "Lebanese-Americans For Change", "Reform Lebanon");
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online", MeetingAgenda.EmptyAgenda());
            meeting.AddAttendee(mo);
            Assert.IsTrue(meeting.HasQuorum());
        }
        
        [TestMethod]
        public void TestNoQuorum()
        {
            var mo = new Person("Mo", "mo@gmail.com");
            var omar = new Person("Omar", "omar@gmail.com");
            var group = Group.NewInstance(mo, "Lebanese-Americans For Change", "Reform Lebanon", new List<Person>() { omar });
            group.AddMember(omar);
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online", MeetingAgenda.EmptyAgenda());
            meeting.AddAttendee(mo);
            Assert.IsFalse(meeting.HasQuorum());
        }
        
        [TestMethod]
        public void TestAttendance()
        {
            var mo = new Person("Mo", "mo@gmail.com");
            var omar = new Person("Omar", "omar@gmail.com");
            var group = Group.NewInstance(mo, "Lebanese-Americans For Change", "Reform Lebanon", new List<Person>() { omar });
            group.AddMember(omar);
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online", MeetingAgenda.EmptyAgenda());
            meeting.AddAttendee(mo);
            meeting.AddAttendee(mo);
            meeting.AddAttendee(mo);
            Assert.AreEqual(1, meeting.Attendees.Count);
        }
    }
}