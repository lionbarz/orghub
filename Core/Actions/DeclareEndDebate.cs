namespace Core.Actions
{
    public class DeclareEndDebate : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} declared that debate has ended.";
        }
    }
}