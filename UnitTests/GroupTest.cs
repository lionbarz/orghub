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
    }
}