namespace Core.Motions
{
    /// <summary>
    /// As defined by Robert's Rules of Order, these are Main Motions.
    /// This class is to differentiate main motions from others by
    /// implementing this interface. Main motions can be moved when
    /// there is no other motion being considered.
    /// </summary>
    public interface IMainMotion : IMotion
    {
        
    }
}