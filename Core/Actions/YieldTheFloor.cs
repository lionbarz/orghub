namespace Core.Actions
{
    public class YieldTheFloor : IAction
    {
        public bool IsAvailableToChairs => true;
        public bool IsAvailableToMembers => true;
        public bool IsAvailableToGuests => true;
        
        public string Describe(Person person)
        {
            return $"{person.Name} yielded the floor.";
        }
    }
}