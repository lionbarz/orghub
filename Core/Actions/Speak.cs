namespace Core.Actions
{
    /// <summary>
    /// Rising and addressing the assembly and speaking.
    /// </summary>
    public class Speak : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} started speaking.";
        }
    }
}