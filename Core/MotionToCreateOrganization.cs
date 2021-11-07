using System.Threading.Tasks;

namespace Core
{
    public class MotionToCreateOrganization : Motion
    {
        private Bylaws Bylaws { get; set; }
        public MotionToCreateOrganization(Person mover, Bylaws bylaws) : base(mover)
        {
            Bylaws = bylaws;
        }
        
        public override string GetText()
        {
            return $"Create organization {Bylaws.Name} with mission {Bylaws.Mission}";
        }

        public override Task OnPassage()
        {
            return Task.CompletedTask;
        }
    }
}