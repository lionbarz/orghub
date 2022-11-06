namespace Core.Motions
{
    /// <summary>
    /// A motion that can be moved by a member and voted
    /// on, and possibly debated.
    /// </summary>
    public interface IMotion
    {
        /// <summary>
        /// The official text of this action which is exactly what a group is bound by.
        /// The format is such that it completes the sentence "there is a motion..."
        /// Examples:
        /// - "that Rami chairs the group"
        /// - "to adjourn the meeting"
        /// - "to adopt the following: 'blah'"
        /// </summary>
        public string GetText();
    }
}