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

            //YesNoBallotBox yesNoBallotBox = new YesNoBallotBox(members);
            //yesNoBallotBox.RecordVote(mo, VoteType.Aye);
            //yesNoBallotBox.RecordVote(faker, VoteType.Aye);
            //Assert.AreEqual(1, yesNoBallotBox.EligibleVoters.Count, "Different voter count");
            //Assert.AreEqual((uint)1, yesNoBallotBox.NumAye, "Different number of ayes");
            //Assert.AreEqual((uint)0, yesNoBallotBox.NumAbstain, "Different number of nays");
            //Assert.AreEqual(VoteResult.AyesHaveIt, yesNoBallotBox.Result);
        }
    }
}