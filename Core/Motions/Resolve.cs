using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// Resolve whatever the resolution text says.
    /// </summary>
    public class Resolve : GroupModifyingMotion
    {
        private string Resolution { get; }

        public Resolve(string resolution, IGroupModifier groupModifier) : base(groupModifier)
        {
            Resolution = resolution;
        }

        public override string GetText()
        {
            return Resolution;
        }   

        public override Task TakeActionAsync()
        {
            GroupModifier.AddResolution(Resolution);
            return Task.CompletedTask;
        }
    }
}