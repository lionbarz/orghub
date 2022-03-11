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
            var group = Group.MassMeeting(mo);
            
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
            var members = new Person[] { mo };
            var group = Group.MassMeeting(mo);

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
    }
}