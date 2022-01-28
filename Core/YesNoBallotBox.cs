using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Takes ballots, voter eligibility, and vote closure,
    /// and provides whether a vote is adopted or dropped
    /// using 50% + 1.
    /// </summary>
    public class YesNoBallotBox
    {
        // The ID of this ballot box.
        public Guid Id { get; }
        
        // How many ballots count towards Aye.
        public int NumAye => CountEligibleBallotsOfType(VoteType.Aye);
        
        // How many ballots count towards Nay.
        public int NumNay => CountEligibleBallotsOfType(VoteType.Nay); 
        
        // How many ballots count towards Abstain.
        public int NumAbstain => CountEligibleBallotsOfType(VoteType.Abstain);

        // Who is eligible to vote.
        private readonly IVoterEligibilityVerifier EligibilityVerifier;
        
        // True if voting is now closed.
        private bool IsClosed { get; set; }

        // How each voter voted.
        private ICollection<YesNoBallot> Ballots { get; }

        public YesNoBallotBox(IVoterEligibilityVerifier eligibilityVerifier)
        {
            Id = Guid.NewGuid();
            IsClosed = false;
            Ballots = new LinkedList<YesNoBallot>();
            EligibilityVerifier = eligibilityVerifier;
        }

        /// <summary>
        /// Records a person's vote.
        /// </summary>
        /// <remarks>
        /// The ballot is remembered, but doesn't mean it will count
        /// towards the final result. That depends on eligibility and
        /// possibly other criteria.
        /// </remarks>
        public void CastBallot(YesNoBallot ballot)
        {
            Ballots.Add(ballot);
        }

        /// <summary>
        /// Closes voting, which does not allow any more ballots to be cast.
        /// Once closed, voting cannot be opened again because it allows the
        /// results to be determined and announced.
        /// </summary>
        public void CloseVoting()
        {
            IsClosed = true;
        }

        /// <summary>
        /// Get the status of the ballot box.
        /// </summary>
        public VoteResult GetStatus()
        {
            if (!IsClosed)
            {
                return VoteResult.Unknown;
            }

            return NumAye > (NumNay + NumAbstain) ? VoteResult.AyesHaveIt : VoteResult.NaysHaveIt;
        }

        /// <summary>
        /// The number of ballots that count for the given type of vote.
        /// </summary>
        private int CountEligibleBallotsOfType(VoteType type)
        {
            var count = 0;
            HashSet<Person> alreadyVoted = new();
            
            foreach (var ballot in Ballots)
            {
                // One person, one vote.
                if (ballot.VoteType == type && EligibilityVerifier.IsEligible(ballot.Voter) &&
                    !alreadyVoted.Contains(ballot.Voter))
                {
                    alreadyVoted.Add(ballot.Voter);
                    count++;
                }    
            }

            return count;
        }
    }
}