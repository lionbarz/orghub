using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// The status of a vote. Whether it's ongoing or completed,
    /// what the tally is, etc.
    /// </summary>
    public class Vote
    {
        // The ID of this vote.
        public Guid Id { get; init; }
        
        // The result of the vote.
        public VoteResult Result { get; private set; }
        
        // How many voted Aye.
        public uint NumAye { get; private set; }
        
        // How many voted Nay.
        public uint NumNay { get; private set; }
        
        // How many voted to abstain.
        public uint NumAbstain { get; private set; }

        // Who is eligible to vote.
        public ICollection<Person> EligibleVoters { get; }

        // How each voter voted.
        private Dictionary<Person, VoteType> VotesCast { get; }

        public Vote(ICollection<Person> eligibleVoters)
        {
            Id = Guid.NewGuid();
            Result = VoteResult.Unknown;
            VotesCast = new Dictionary<Person, VoteType>();
            EligibleVoters = eligibleVoters;
        }

        /// <summary>
        /// True if the vote is done and finished.
        /// </summary>
        public bool IsVoteCompleted() => Result != VoteResult.Unknown;
        
        /// <summary>
        /// Casts a vote by a person for a vote.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="voteId"></param>
        /// <param name="voteType"></param>
        public void RecordVote(Person person, VoteType voteType)
        {
            if (!EligibleVoters.Contains(person))
            {
                //throw new ArgumentException($"{person.Name} is not eligible to vote.");
                // Throw the vote away. TODO: Return a status that says that.
                return;
            }
            
            VotesCast[person] = voteType;
            NumAye = CountVotesOfType(VoteType.Aye);
            NumNay = CountVotesOfType(VoteType.Nay);
            NumAbstain = CountVotesOfType(VoteType.Abstain);

            if (VotesCast.Count == EligibleVoters.Count)
            {
                // Simple majority for now.
                Result = (NumAye > NumNay) ? VoteResult.AyesHaveIt : VoteResult.NaysHaveIt;
            }
        }
        
        /// <summary>
        /// How many people voted a certain way.
        /// </summary>
        private uint CountVotesOfType(VoteType type)
        {
            var count = VotesCast?.Count(x => x.Value == type) ?? 0;
            return (uint)count;
        }
    }
}