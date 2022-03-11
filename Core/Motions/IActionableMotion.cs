using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// A motion, when passed, can do something.
    /// </summary>
    public interface IActionableMotion : IMotion
    {
        Task TakeActionAsync(IGroupModifier groupModifier);
    }
}