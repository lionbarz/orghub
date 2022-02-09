namespace Core.Motions
{
    /// <summary>
    /// Amending one action with something else..
    /// </summary>
    /// <typeparam name="T">The type of action being amended.</typeparam>
    public class Amend<T> : MotionBase where T : MotionBase
    {
        private T OriginalAction { get; set; }
        private T AmendedAction { get; set; }

        public Amend(Person mover, T originalAction, T amendedAction) : base(mover)
        {
            OriginalAction = originalAction;
            AmendedAction = amendedAction;
        }

        public override string GetText()
        {
            return $"Replace \"{OriginalAction.GetText()}\" with \"{AmendedAction.GetText()}\"";
        }
    }
}