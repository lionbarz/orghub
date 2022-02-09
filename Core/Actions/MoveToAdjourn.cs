namespace Core.Actions
{
    public class MoveToAdjourn : IAction
    {
        public bool IsAvailableToChairs => true;
        public bool IsAvailableToMembers => true;
        public bool IsAvailableToGuests => false;
        
        public string Describe(Person person)
        {
            return $"{person.Name} moved to adjourn.";
        }
    }
}