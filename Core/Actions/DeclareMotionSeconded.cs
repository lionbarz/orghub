namespace Core.Actions
{
    public class DeclareMotionSeconded : IAction
    {
        public string RecordEntry(Person person)
        {
            return $"{person.Name} declared that the motion was seconded.";
        }
    }
}