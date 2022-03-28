namespace Core.Actions
{
    public class MoveToAdjourn : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} moved to adjourn.";
        }
    }
}