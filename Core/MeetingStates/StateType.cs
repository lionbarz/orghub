namespace Core.MeetingStates
{
    /// <summary>
    /// Used to identify states. These are sent to the
    /// frontend so it can customize view based on the
    /// current state.
    /// </summary>
    public enum StateType
    {
        Adjourned,
        Debate,
        MotionProposed,
        OpenFloor,
        SpeakerHasFloor,
        Voting
    }
}