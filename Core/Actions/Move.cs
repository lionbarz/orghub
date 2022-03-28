using Core.Motions;

namespace Core.Actions
{
    /// <summary>
    /// Move any arbitrary motion.
    /// </summary>
    public class Move : IAction
    {
        /// <summary>
        /// Description of what the person is proposing/moving.
        /// </summary>
        public IMotion Motion { get; }

        public Move(IMotion motion)
        {
            Motion = motion;
        }

        public string RecordEntry(Person person)
        {
            return $"{person.Name} moved that: {Motion.GetText()}";
        }
    }
}