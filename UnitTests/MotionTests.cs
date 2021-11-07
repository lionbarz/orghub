using System;
using System.Collections.Generic;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MotionTests
    {
        /// <summary>
        /// Tests all the statuses of a motion.
        /// </summary>
        [TestMethod]
        public void TestMotionStatus()
        {
            var person = new Person();
            var motion = new MotionToAdjourn(person, DateTimeOffset.Now);
            Assert.AreEqual(MotionStatus.Introduced, motion.GetStatus());
            var vote = new Vote(new List<Person>() { person });
            motion.Vote = vote;
            Assert.AreEqual(MotionStatus.Voting, motion.GetStatus());
            vote.RecordVote(person, VoteType.Aye);
            Assert.AreEqual(MotionStatus.Adopted, motion.GetStatus());
            
            // TODO: this shouldn't work -- the voting should be closed!
            vote.RecordVote(person, VoteType.Nay);
            Assert.AreEqual(MotionStatus.Dropped, motion.GetStatus());
        }
    }
}