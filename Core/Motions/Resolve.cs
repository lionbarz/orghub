using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// Resolve whatever the resolution text says.
    /// </summary>
    public class Resolve : IActionableMotion
    {
        private string Resolution { get; }

        public Resolve(string resolution)
        {
            Resolution = resolution;
        }

        public string GetText()
        {
            return Resolution;
        }

        public async Task TakeActionAsync(IGroupModifier groupModifier)
        {
            groupModifier.AddResolution(Resolution);
        }
    }
}