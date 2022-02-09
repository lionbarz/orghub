namespace Core.Actions
{
    /// <summary>
    /// Rising and addressing the assembly and speaking.
    /// </summary>
    public class Speak : IAction
    {
        public bool IsAvailableToChairs => true;
        public bool IsAvailableToMembers => true;
        public bool IsAvailableToGuests => true;
        
        public string Describe(Person person)
        {
            return $"{person.Name} started speaking.";
        }
    }
}