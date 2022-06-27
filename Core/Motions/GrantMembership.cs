using System.Threading.Tasks;

namespace Core.Motions
{
    public class GrantMembership : GroupModifyingMotion, IMainMotion
    {
        private readonly Person _applicant;
        
        public GrantMembership(Person applicant, IGroupModifier groupModifier) : base(groupModifier)
        {
            _applicant = applicant;
        }
        
        public override string GetText()
        {
            return $"Make {_applicant.Name} a member of {GroupModifier.GetName()}.";
        }

        public override Task TakeActionAsync()
        {
            GroupModifier.AddMember(_applicant);
            GroupModifier.AddResolution(GetText());
            return Task.CompletedTask;
        }
    }
}