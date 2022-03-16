using Core.Actions;

namespace Core.Motions
{
    /// <summary>
    /// A motion, when passed, takes an action.
    /// This allows affecting the state and produces of a meeting.
    /// </summary>
    public interface IActionTakingMotion
    {
        IAction GetAction();
    }
}