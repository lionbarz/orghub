using Core;
using Core.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class GroupTests
    {
        public void ActiveMotion_OneMotion()
        {
            // Stage
            var group = new Group();
            var person = new Person();
            var action = new Resolve("Live Laugh Love");
            var motion = new Motion(person, action);
            
            // Act
            group.Move(motion);
            
            // Verify
            Assert.AreEqual(motion, group.GetActiveMotion());
        }
        
        public void ActiveMotion_TwoMotions()
        {
            // Stage
            var group = new Group();
            var person = new Person();
            var action = new Resolve("Live Laugh Love");
            var motion = new Motion(person, action);
            
            // Act
            group.Move(motion);
            var newAction = new Resolve("Play Towerfall");
            var amendment = new Amend<Resolve>(action, newAction);
            var newMotion = new Motion(person, amendment);
            group.Move(newMotion);
            
            // Verify
            Assert.AreEqual(newMotion, group.GetActiveMotion());
        }
        
        public void CannotAmendWrongMotion()
        {
            // Stage
            var group = new Group();
            var person = new Person();
            var action = new Resolve("Live Laugh Love");
            var motion = new Motion(person, action);
            
            // Act
            group.Move(motion);
            var newAction = new Resolve("Play Towerfall");
            var amendment = new Amend<Resolve>(action, newAction);
            var newMotion = new Motion(person, amendment);
            group.Move(newMotion);

            // Verify
            Assert.AreEqual(newMotion, group.GetActiveMotion());
        }
    }
}