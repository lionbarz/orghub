using Core;
using Core.Actions;
using Core.MeetingStates;
using Core.Motions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GroupTest
    {
        [TestMethod]
        public void MassMeetingAttendance()
        {
            var mo = new Person("Mo");
            var zaki = new Person("Zaki");
            var members = new Person[] { mo };
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
        public void MassMeetingSetChair()
        {
            var mo = new Person("Mo");
            var omar = new Person("Omar");
            var group = Group.NewInstance(mo);

            group.TakeAction(mo, new CallMeetingToOrder());
            Assert.IsInstanceOfType(group.GetState(), typeof(OpenFloorState));
            
            group.TakeAction(mo, new Move(new ElectChair(omar)));
            Assert.IsInstanceOfType(group.GetState(), typeof(DebateState));
            
            group.TakeAction(mo, new DeclareMotionPassed());
            Assert.AreEqual(omar, group.Chair);
            Assert.IsInstanceOfType(group.GetState(), typeof(OpenFloorState));

            group.TakeAction(mo, new MoveToAdjourn());
            Assert.IsInstanceOfType(group.GetState(), typeof(AdjournedState));
        }
        
        [TestMethod]
        public void SecondAndVoteOnMotion()
        {
            var mo = new Person("Mo");
            var roni = new Person("Roni");
            var members = new Person[] { mo };
            var group = Group.NewInstance(mo);

            group.TakeAction(mo, new CallMeetingToOrder());
            Assert.IsInstanceOfType(group.GetState(), typeof(OpenFloorState));
            
            group.TakeAction(mo, new Move(new ElectChair(roni)));
            Assert.IsInstanceOfType(group.GetState(), typeof(DebateState));
            
            group.TakeAction(roni, new Move(new EndDebate()));
            Assert.IsInstanceOfType(group.GetState(), typeof(MotionProposed));

            // Seconded, so a vote is taken on ending debate.
            group.TakeAction(mo, new SecondMotion());
            Assert.IsInstanceOfType(group.GetState(), typeof(VotingState));
            
            // Vote passes, so debate ends and now voting on original motion.
            group.TakeAction(mo, new DeclareMotionPassed());
            Assert.IsInstanceOfType(group.GetState(), typeof(VotingState));
            
            // Original motion passed, so back to open floor.
            group.TakeAction(mo, new DeclareMotionPassed());
            Assert.AreEqual(roni, group.Chair);
            Assert.IsInstanceOfType(group.GetState(), typeof(OpenFloorState));
        }
    }
}