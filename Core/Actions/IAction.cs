namespace Core.Actions
{
    public interface IAction
    {
        /// <summary>
        /// The official text of this action which
        /// is exactly what a group is bound by.
        /// </summary>
        public abstract string GetText();
    }
}