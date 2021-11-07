using System.Threading.Tasks;

namespace Core.Actions
{
    public class CreateOrganization : IAction
    {
        private Bylaws Bylaws { get; }
        
        public CreateOrganization(Bylaws bylaws)
        {
            Bylaws = bylaws;
        }
        
        public string GetText()
        {
            return $"Create organization {Bylaws.Name} with mission {Bylaws.Mission}";
        }
    }
}