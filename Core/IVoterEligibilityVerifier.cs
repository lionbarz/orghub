namespace Core
{
    public interface IVoterEligibilityVerifier
    {
        /// <summary>
        /// True if the person is eligible to vote.
        /// </summary>
        bool IsEligible(Person person);
    }
}