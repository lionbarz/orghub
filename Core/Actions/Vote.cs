namespace Core.Actions
{
    /// <summary>
    /// Vote in some way (aye, nay, abstain).
    /// </summary>
    public class Vote : IAction
    {
        public VoteType Type { get; private init; }
        
        public Vote(VoteType type)
        {
            Type = type;
        }
        
        public string RecordEntry(Person person)
        {
            return $"{person.Name} voted yes.";
        }
    }
}