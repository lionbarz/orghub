using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// Resolve whatever the resolution text says.
    /// </summary>
    public class Resolve : IGroupModifyingMotion, IMainMotion
    {
        private string Resolution { get; }

        public Resolve(string resolution)
        {
            Resolution = resolution;
        }

        public string GetText()
        {
            return $"adopt resolution: \"{Resolution}\"";
        }   

        public Task TakeActionAsync(IGroupModifier groupModifier)
        {
            groupModifier.AddResolution(Resolution);
            return Task.CompletedTask;
        }
    }
}