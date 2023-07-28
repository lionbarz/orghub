using System.Threading.Tasks;

namespace Core.Motions
{
    public class ChangeOrgName : IGroupModifyingMotion, IMainMotion
    {
        private readonly string _suggestedName;

        public ChangeOrgName(string suggestedName)
        {
            _suggestedName = suggestedName;
        }

        public string GetText()
        {
            return $"Change the name of the group to \"{_suggestedName}\"";
        }

        public Task TakeActionAsync(IGroupModifier groupModifier)
        {
            groupModifier.SetName(_suggestedName);
            groupModifier.AddResolution(GetText());
            return Task.CompletedTask;
        }
    }
}