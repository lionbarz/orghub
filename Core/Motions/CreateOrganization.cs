using System.Threading.Tasks;

namespace Core.Motions
{
    public class CreateOrganization : IMotion
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