using System;
using System.Collections.Generic;
using Core;
using Core.Motions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GroupTest
    {
        [TestMethod]
        public void GroupAttendance()
        {
            var mo = new Person("Mo");
            var roni = new Person("Roni");
            var group = Group.NewInstance(mo, "Alliance for Lebanese Reform", "Defeating the corrupt Lebanese warlords that are controlling the country.");
            Assert.IsFalse(group.IsMember(roni.Id));
            Assert.IsTrue(group.IsMember(mo.Id));
        }
        
        [TestMethod]
        public void ElectChair()
        {
            var mo = new Person("Mo");
            var omar = new Person("Omar");
            var group = Group.NewInstance(mo, "Lebanese-American Union", "Reform Lebanon", new List<Person>() { omar });
            group.AddMember(omar);
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online");
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
            var mo = new Person("Mo");
            var omar = new Person("Omar");
            var group = Group.NewInstance(mo, "Lebanese-Americans For Change", "Reform Lebanon", new List<Person>() { omar });
            group.AddMember(omar);
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online");
            meeting.AddAttendee(mo);
            meeting.AddAttendee(omar);
            Assert.IsTrue(meeting.HasQuorum());
        }
        
        [TestMethod]
        public void TestNoQuorum()
        {
            var mo = new Person("Mo");
            var omar = new Person("Omar");
            var group = Group.NewInstance(mo, "Lebanese-Americans For Change", "Reform Lebanon", new List<Person>() { omar });
            group.AddMember(omar);
            var meeting = Meeting.NewInstance(group, DateTimeOffset.Now, "talknstuff", "online");
            meeting.AddAttendee(mo);
            Assert.IsFalse(meeting.HasQuorum());
        }
    }
}