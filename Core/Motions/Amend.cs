namespace Core.Motions
{
    /// <summary>
    /// Amending one action with something else..
    /// </summary>
    /// <typeparam name="T">The type of action being amended.</typeparam>
    public class Amend<T> : IMotion where T : IMotion
    {
        private T OriginalAction { get; set; }
        private T AmendedAction { get; set; }

        public Amend(T originalAction, T amendedAction)
        {
            OriginalAction = originalAction;
            AmendedAction = amendedAction;
        }

        public string GetText()
        {
            return $"Replace \"{OriginalAction.GetText()}\" with \"{AmendedAction.GetText()}\"";
        }
    }
}