namespace Core.Actions
{
    /// <summary>
    /// Amending one action with something else..
    /// </summary>
    /// <typeparam name="T">The type of action being amended.</typeparam>
    public class Amend<T> : IAction where T : IAction
    {
        private T OriginalAction { get; set; }
        private T AmendedAction { get; set; }

        public Amend(T originalAction, T amendedActionAction)
        {
            OriginalAction = originalAction;
            AmendedAction = amendedActionAction;
        }

        public string GetText()
        {
            return $"Replace \"{OriginalAction.GetText()}\" with \"{AmendedAction.GetText()}\"";
        }
    }
}