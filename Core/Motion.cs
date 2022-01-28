using System;
using Core.Actions;

namespace Core
{
    /// <summary>
    /// A motion that can be moved by a member of a group
    /// for a particular action.
    /// </summary>
    internal class Motion
    {
        /// <summary>
        /// Uniquely identifies this motion.
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// The action that is being moved.
        /// </summary>
        public IAction Action { get; private set; }
        
        /// <summary>
        /// The person who initiated the motion.
        /// </summary>
        public Person Mover { get; private init; }

        /// <summary>
        /// The vote for the current motion, or null if hasn't been voted on.
        /// </summary>
        public YesNoBallotBox? BallotBox { get; private set; }

        /// <summary>
        /// Starts the vote on this motion, which creates a ballot box to start
        /// accepting ballots.
        /// </summary>
        /// <param name="eligibilityVerifier">How to verify vote eligibility.</param>
        /// <exception cref="Exception">If voting has already started.</exception>
        public void StartVote(IVoterEligibilityVerifier eligibilityVerifier)
        {
            if (BallotBox != null)
            {
                throw new ApplicationException("Voting has already started.");
            }

            BallotBox = new YesNoBallotBox(eligibilityVerifier);
        }

        /// <summary>
        /// Create a new motion.
        /// </summary>
        /// <param name="mover">Whoever moved this motion.</param>
        /// <param name="action">The action that is being moved.</param>
        public Motion(Person mover, IAction action)
        {
            Id = Guid.NewGuid();
            Mover = mover;
            Action = action;
        }
    }
}