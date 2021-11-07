namespace Core
{
    /// <summary>
    /// Something that can be voted on.
    /// </summary>
    public interface IVotable
    {
        // The official text of the thing being voted on.
        public string GetOfficialText();
    }
}