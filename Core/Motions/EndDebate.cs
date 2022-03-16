using Core.Actions;

namespace Core.Motions
{
    /// <summary>
    /// Call the question?
    /// </summary>
    public class EndDebate : IMotion, IActionTakingMotion
    {
        public string GetText()
        {
            return $"Stop debating and vote.";
        }

        public IAction GetAction()
        {
            return new DeclareEndDebate();
        }
    }
}