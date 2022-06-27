namespace Core.Motions
{
    /// <summary>
    /// Allows us to distinguish subsidiary motions (amend, change rules of debate, end debate).
    /// </summary>
    public interface ISubsidiaryMotion : IMotion
    {
    }
}