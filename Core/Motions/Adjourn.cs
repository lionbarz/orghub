
namespace Core.Motions
{
    /// <summary>
    /// Moves to adjourn the meeting.
    /// </summary>
    public class Adjourn : MotionBase, IPrivilegedMotion
    {
        public Adjourn(Person mover) : base(mover)
        {
        }

        public override string GetText()
        {
            return "adjourn the meeting";
        }
    }
}