using System.Linq;
using Core;
using Core.Actions;
using Core.Meetings;
using Core.MeetingStates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class StateManagerTest
    {
        /// <summary>
        /// Goes to the next state when an action is taken.
        /// When a person speaks, they can then yield. 
        /// </summary>
        [TestMethod]
        public void CanYieldAfterSpeaking()
        {
            Mock<IMinuteRecorder> mockMinuteRecorder = new Mock<IMinuteRecorder>();
            var agenda = MeetingAgenda.EmptyAgenda();
            var openFloorState = OpenFloorState.InstanceOf(new TestGroupModifier(), agenda, mockMinuteRecorder.Object);
            var stateManager = StateManager.StartingWithState(openFloorState);
            var personRole = MeetingAttendee.AsMember(new Person());
            stateManager.Speak(personRole);
            var canYield = stateManager.GetActionSupportForPerson(personRole).First(x => x.Action == Action.Yield)
                .IsSupported;
            Assert.IsTrue(canYield);
        }
    }
}