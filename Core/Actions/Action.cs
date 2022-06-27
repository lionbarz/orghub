namespace Core.Actions
{
    /// <summary>
    /// Actions that can be taken by people in a meeting, of all roles.
    /// </summary>
    public enum Action
    {
        CallToOrder,
        DeclareTimeExpired,
        MoveMainMotion,
        MoveToAdjourn,
        MoveSubsidiaryMotion,
        Second,
        Speak,
        Vote,
        Yield
    }
}