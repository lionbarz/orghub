using System.Threading.Tasks;

namespace Core.Motions
{
    public class GrantMembership : IGroupModifyingMotion, IMainMotion
    {
        private readonly Person _applicant;
        
        public GrantMembership(Person applicant, IGroupModifier groupModifier)
        {
            _applicant = applicant;
        }
        
        public string GetText()
        {
            return $"Grant membership to {_applicant.Name}.";
        }

        public Task TakeActionAsync(IGroupModifier groupModifier)
        {
            groupModifier.AddMember(_applicant);
            groupModifier.AddResolution(GetText());
            return Task.CompletedTask;
        }
    }
}