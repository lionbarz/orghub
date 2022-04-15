using System.Threading.Tasks;

namespace Core.Motions
{
    public abstract class GroupModifyingMotion : IMotion
    {
        protected IGroupModifier GroupModifier { get; private init; }
        
        protected GroupModifyingMotion(IGroupModifier groupModifier)
        {
            GroupModifier = groupModifier;
        }

        public abstract string GetText();
        
        public abstract Task TakeActionAsync();
    }
}