namespace Core
{
    /// <summary>
    /// Calculates whether a quorum is achieved.
    /// </summary>
    public interface IQuorumCalculator
    {
        /// <summary>
        /// Given the number of present voting members,
        /// whether quorum is achieved.
        /// </summary>
        public bool IsAchieved(int numPresent);
    }
}