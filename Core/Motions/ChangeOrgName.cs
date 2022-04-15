using System.Threading.Tasks;

namespace Core.Motions
{
    public class ChangeOrgName : GroupModifyingMotion
    {
        private readonly string _suggestedName;

        public ChangeOrgName(string suggestedName, IGroupModifier groupModifier) : base(groupModifier)
        {
            _suggestedName = suggestedName;
        }

        public override string GetText()
        {
            return $"Change the name of the group to \"{_suggestedName}\"";
        }

        public override Task TakeActionAsync()
        {
            GroupModifier.SetName(_suggestedName);
            GroupModifier.AddResolution(GetText());
            return Task.CompletedTask;
        }
    }
}