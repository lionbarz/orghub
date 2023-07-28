using System.Threading.Tasks;

namespace Core.Motions
{
    public interface IGroupModifyingMotion : IMotion
    {
        public Task TakeActionAsync(IGroupModifier groupModifier);
    }
}