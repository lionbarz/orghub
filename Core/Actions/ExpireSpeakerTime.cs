namespace Core.Actions
{
    public class ExpireSpeakerTime : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} notified the speaker that their time has expired.";
        }
    }
}