namespace Core.Actions
{
    public class CallMeetingToOrder : IAction
    {
        public bool IsAvailableToChairs => true;
        public bool IsAvailableToMembers => false;
        public bool IsAvailableToGuests => false;
        
        public string Describe(Person person)
        {
            return $"{person.Name} called the meeting to order.";
        }
    }
}