using System;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// Represents any motion that can be moved by a member of a group.
    /// </summary>
    public abstract class Motion
    {
        /// <summary>
        /// Uniquely identifies this motion.
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// The vote for the current motion, or null if there's no vote.
        /// </summary>
        private Vote? _vote;
        
        /// <summary>
        /// The person who initiated the motion.
        /// </summary>
        public Person Mover { get; private init; }
        
        /// <summary>
        /// The official text of this motion which
        /// describes it in detail and explains it.
        /// </summary>
        public abstract string GetText();

        /// <summary>
        /// Actions to perform when the motion passes.
        /// TODO: Pass whatever interfaces/objects it needs to act on.
        /// </summary>
        public abstract Task OnPassage();

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
        protected Motion(Person mover)
        {
            Id = Guid.NewGuid();
            Mover = mover;
        }
    }
}