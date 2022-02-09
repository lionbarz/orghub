using System.Threading.Tasks;

namespace Core.Motions
{
    public class CreateOrganization : MotionBase
    {
        private Bylaws Bylaws { get; }
        
        public CreateOrganization(Person mover, Bylaws bylaws) : base(mover)
        {
            Bylaws = bylaws;
        }
        
        public override string GetText()
        {
            return $"Create organization {Bylaws.Name} with mission {Bylaws.Mission}";
        }
    }
}