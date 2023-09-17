namespace Core
{
    public interface IMinuteRecorder
    {
        /// <summary>
        /// Add something to the meeting minutes.
        /// In the future, this can be structured, with fields for
        /// agenda items, actors, etc. so they can be linked.
        /// </summary>
        void RecordMinute(string text);
    }
}