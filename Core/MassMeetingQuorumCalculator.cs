namespace Core
{
    public class MassMeetingQuorumCalculator : IQuorumCalculator
    {
        /// <summary>
        /// In a mass meeting, all the people present constitute
        /// the membership, so quorum is always achieved.
        /// </summary>
        public bool IsAchieved(int numPresent)
        {
            return true;
        }
    }
}