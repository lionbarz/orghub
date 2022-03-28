
namespace Core.Actions
{
    /// <summary>
    /// Actions that can be taken during a meeting, such
    /// making a motion, rising to speak, calling the
    /// meeting to order, etc.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// What should go in the record/minutes, in past tense.
        /// </summary>
        /// <remarks>Example: Mo started speaking.</remarks>
        public string RecordEntry(Person person);
    }
}