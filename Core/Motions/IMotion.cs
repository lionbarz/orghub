namespace Core.Motions
{
    /// <summary>
    /// A motion that can be moved by a member and voted
    /// on, and possibly debated.
    /// </summary>
    public interface IMotion
    {
        /// <summary>
        /// The official text of this action which
        /// is exactly what a group is bound by.
        /// </summary>
        public string GetText();
    }
}