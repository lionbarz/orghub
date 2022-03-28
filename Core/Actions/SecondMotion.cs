namespace Core.Actions
{
    /// <summary>
    /// The action of seconding a motion.
    /// </summary>
    public class SecondMotion : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} seconded the motion.";
        }
    }
}