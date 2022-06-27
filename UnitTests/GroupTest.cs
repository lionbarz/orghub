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
            var zaki = new Person("Zaki");
            var group = Group.NewInstance(mo);
            
            Assert.IsTrue(group.IsMember(zaki.Id));
            Assert.IsTrue(group.IsMember(mo.Id));
        }
        
        [TestMethod]
        public void RegularGroupAttendance()
        {
            var mo = new Person("Mo");
            var zaki = new Person("Zaki");
            var members = new Person[] { mo };
            var group = Group.WithMembership(members);
            
            Assert.IsFalse(group.IsMember(zaki.Id));
            Assert.IsTrue(group.IsMember(mo.Id));
        }
        
        [TestMethod]
        public void GroupSetChair()
        {
            var mo = new Person("Mo");
            var omar = new Person("Omar");
            var group = Group.NewInstance(mo, new List<Person>() { omar });
            group.AddMember(omar);
            var moRole = group.CreatePersonRole(mo);
            var omarRole = group.CreatePersonRole(omar);
            var meeting = Meeting.NewInstance(group.Id, group, mo, DateTimeOffset.Now, 0);

            meeting.State.CallMeetingToOrder(moRole);
            
            /*
            group.TakeAction(mo, new Move(new ElectChair(omar, group)));
            Assert.IsInstanceOfType(group.GetState(), typeof(DebateState));
            
            group.TakeAction(mo, new DeclareMotionPassed());
            Assert.AreEqual(omar, group.Chair);
            Assert.IsInstanceOfType(group.GetState(), typeof(OpenFloorState));

            group.TakeAction(mo, new MoveToAdjourn());
            Assert.IsInstanceOfType(group.GetState(), typeof(AdjournedState));
            */
        }
        
        [TestMethod]
        public void SecondAndVoteOnMotion()
        {
            var mo = new Person("Mo");
            var omar = new Person("Omar");
            var group = Group.NewInstance(mo, new List<Person>() { omar });
            group.AddMember(omar);
            var moRole = group.CreatePersonRole(mo);
            var omarRole = group.CreatePersonRole(omar);
            var meeting = Meeting.NewInstance(group.Id, group, mo, DateTimeOffset.Now, 0);

            meeting.State.CallMeetingToOrder(moRole);
            meeting.State.MoveMainMotion(omarRole, new ElectChair(omar, group));
            meeting.State.Second(moRole);
            
            // Moves to a vote.
            meeting.State.DeclareTimeExpired(moRole);

            meeting.State.Vote(moRole, VoteType.Aye);
            meeting.State.Vote(omarRole, VoteType.Aye);
            meeting.State.DeclareTimeExpired(moRole);
            
            Assert.AreEqual(omar, group.Chair);
        }
    }
}