using System.Collections.Generic;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class VoteTests
    {
        /// <summary>
        /// Requirement: Someone can call for a vote from specific people to create
        /// a new organization with a certain name.
        /// TODO: If the vote passes, the organization is ... saved to the database?
        /// TODO: People can see a description of what they're voting on.
        /// </summary>
        [TestMethod]
        public void Test_Vote()
        {
            Person mo = new()
            {
                Name = "Mohamed Fakhreddine",
                Email = "mohamed.y.fakhreddine@gmail.com"
            };
            Person faker = new()
            {
                Name = "Faker McFake",
                Email = "faker@gmail.com"
            };
            var members = new List<Person>() { mo };

            Vote vote = new Vote(members);
            vote.RecordVote(mo, VoteType.Aye);
            vote.RecordVote(faker, VoteType.Aye);
            Assert.AreEqual(1, vote.EligibleVoters.Count, "Different voter count");
            Assert.AreEqual((uint)1, vote.NumAye, "Different number of ayes");
            Assert.AreEqual((uint)0, vote.NumAbstain, "Different number of nays");
            Assert.AreEqual(VoteResult.AyesHaveIt, vote.Result);
        }
    }
}