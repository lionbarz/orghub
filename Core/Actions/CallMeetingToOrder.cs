namespace Core.Actions
{
    public class CallMeetingToOrder : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} called the meeting to order.";
        }

        public bool IsPermitted(PersonRole person)
        {
            return person.IsChair;
        }
    }
}