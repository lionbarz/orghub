using Core;
using Core.Meetings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    /// <summary>
    /// Test things related to the agenda.
    /// </summary>
    [TestClass]
    public class MeetingAgendaTest
    {

        /// <summary>
        /// Tests that calling a meeting to order puts the meeting state
        /// at the first agenda item.
        /// </summary>
        [TestMethod]
        public void CallToOrderTest()
        {
            // TODO: this method
        }

        /// <summary>
        /// Test advancing through agenda items and that
        /// the current item flag is updated correctly.
        /// </summary>
        [TestMethod]
        public void TestCurrentItem()
        {
            var person = new Person("Mo", "mo@mo.com");
            var item1 = new ResolutionAgendaItem(person, "Do stuff.");
            var item2 = new ResolutionAgendaItem(person, "Do more stuff.");
            var agenda = MeetingAgenda.FromItems(new[]
            {
                item1,
                item2
            });
            
            var currentItem = agenda.GetCurrentItem();
            Assert.IsNull(currentItem);
            
            var didAdvance = agenda.MoveToNextItem(out var nextItem);
            Assert.IsTrue(didAdvance);
            currentItem = agenda.GetCurrentItem();
            Assert.IsNotNull(currentItem);
            Assert.AreSame(currentItem, item1);
            Assert.AreSame(currentItem, nextItem);
            Assert.IsTrue(currentItem.IsCurrent);
            
            didAdvance = agenda.MoveToNextItem(out nextItem);
            Assert.IsTrue(didAdvance);
            currentItem = agenda.GetCurrentItem();
            Assert.IsNotNull(currentItem);
            Assert.AreSame(currentItem, item2);
            Assert.AreSame(currentItem, nextItem);
            Assert.IsFalse(item1.IsCurrent);
            Assert.IsTrue(item2.IsCurrent);
            
            didAdvance = agenda.MoveToNextItem(out nextItem);
            Assert.IsFalse(didAdvance);
            currentItem = agenda.GetCurrentItem();
            Assert.IsNull(currentItem);
            Assert.IsFalse(item1.IsCurrent);
            Assert.IsFalse(item2.IsCurrent);
        }
    }
}