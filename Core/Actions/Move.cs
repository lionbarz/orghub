using Core.Motions;

namespace Core.Actions
{
    /// <summary>
    /// Move any arbitrary motion.
    /// </summary>
    public class Move : IAction
    {
        public bool IsAvailableToChairs => true;
        public bool IsAvailableToMembers => true;
        public bool IsAvailableToGuests => false;
        
        /// <summary>
        /// Description of what the person is proposing/moving.
        /// </summary>
        public IMotion Motion { get; }

        public Move(IMotion motion)
        {
            Motion = motion;
        }

        public string Describe(Person person)
        {
            return $"{person.Name} moved that: {Motion.GetText()}";
        }
    }
}