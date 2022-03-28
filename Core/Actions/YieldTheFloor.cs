namespace Core.Actions
{
    public class YieldTheFloor : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} yielded the floor.";
        }
    }
}