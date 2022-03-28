namespace Core.Actions
{
    public class DeclareMotionPassed : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} declared that the motion was carried.";
        }
    }
}