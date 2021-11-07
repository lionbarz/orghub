using System;
using Core.Actions;

namespace Core
{
    /// <summary>
    /// A motion that can be moved by a member of a group
    /// for a particular action.
    /// </summary>
    public class Motion
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
        /// The vote for the current motion, or null if there's no vote.
        /// </summary>
        private Vote? _vote;
        
        /// <summary>
        /// The person who initiated the motion.
        /// </summary>
        public Person Mover { get; private init; }

        /// <summary>
        /// Get the current vote, if any.
        /// </summary>
        /// <exception cref="Exception">If setting a vote that already exists.</exception>
        public Vote? Vote
        {
            get => _vote;
            set
            {
                if (_vote != null)
                {
                    throw new Exception("Voting as already started.");
                }

                _vote = value;
            }
        }

        /// <summary>
        /// Gets the status, ie what stage of its lifetime the motion is in.
        /// </summary>
        public MotionStatus GetStatus()
        {
            if (Vote != null && Vote.IsVoteCompleted())
            {
                return Vote.Result == VoteResult.AyesHaveIt ? MotionStatus.Adopted : MotionStatus.Dropped;
            }
            
            if (Vote != null && !Vote.IsVoteCompleted())
            {
                return MotionStatus.Voting;
            }

            return MotionStatus.Introduced;
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