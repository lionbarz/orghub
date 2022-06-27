using Core.Actions;

namespace Core
{
    /// <summary>
    /// Whether each action that a peron can take in a meeting is supported
    /// or not, plus an explanation of why or why not.
    /// </summary>
    public class ActionSupport
    {
        /// <summary>
        /// The action in question.
        /// </summary>
        public Action Action { get; private init; }
        
        /// <summary>
        /// Whether the action is supported.
        /// </summary>
        public bool IsSupported { get; private init; }
        
        /// <summary>
        ///  Why or why not it's supported.
        /// </summary>
        public string? Explanation { get; private init; }

        public static ActionSupport InstanceOf(Action action, bool isSupported, string explanation)
        {
            return new ActionSupport()
            {
                Action = action,
                IsSupported = isSupported,
                Explanation = explanation
            };
        }
    }
}