namespace Core.Actions
{
    /// <summary>
    /// Actions that can be taken during a meeting, such
    /// making a motion, rising to speak, calling the
    /// meeting to order, etc.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Whether the chair can invoke this action.
        /// </summary>
        /// <returns></returns>
        public bool IsAvailableToChairs();

        /// <summary>
        /// Whether a member can invoke this action.
        /// </summary>
        /// <returns></returns>
        public bool IsAvailableToMembers();

        /// <summary>
        /// Whether a meeting guest can invoke this action.
        /// </summary>
        /// <returns></returns>
        public bool IsAvailableToGuests();
    }
}