namespace Core
{
    /// <summary>
    /// A person and their yes/no vote.
    /// </summary>
    public class YesNoBallot
    {
        /// <summary>
        /// The person voting.
        /// </summary>
        public Person Voter { get; }
        
        /// <summary>
        /// How the person is voting.
        /// </summary>
        public VoteType VoteType { get; }

        public YesNoBallot(Person voter, VoteType voteType)
        {
            Voter = voter;
            VoteType = voteType;
        }
    }
}