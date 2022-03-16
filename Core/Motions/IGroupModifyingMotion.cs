using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// A motion, when passed, changes group properties.
    /// </summary>
    public interface IGroupModifyingMotion : IMotion
    {
        Task TakeActionAsync(IGroupModifier groupModifier);
    }
}