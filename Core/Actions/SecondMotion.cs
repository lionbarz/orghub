namespace Core.Actions
{
    /// <summary>
    /// The action of seconding a motion.
    /// </summary>
    public class SecondMotion : IAction
    {
        public bool IsAvailableToChairs => false;
        public bool IsAvailableToMembers => true;
        public bool IsAvailableToGuests => false;
        public string Describe(Person person)
        {
            return $"{person.Name} seconded the motion.";
        }
    }
}