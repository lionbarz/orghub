namespace Core.Actions
{
    public class DeclareMotionSeconded : IAction
    {
        public bool IsAvailableToChairs => true;
        public bool IsAvailableToMembers => false;
        public bool IsAvailableToGuests => false;
        public string Describe(Person person)
        {
            return $"{person.Name} declared that the motion was seconded.";
        }
    }
}