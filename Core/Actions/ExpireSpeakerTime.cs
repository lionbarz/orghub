namespace Core.Actions
{
    public class ExpireSpeakerTime : IAction
    {
        public bool IsAvailableToChairs => true;
        public bool IsAvailableToMembers => false;
        public bool IsAvailableToGuests => false;
        
        public string Describe(Person person)
        {
            return $"{person.Name} notified the speaker that their time has expired.";
        }
    }
}