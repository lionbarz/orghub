using System.Threading.Tasks;

namespace Core.Actions
{
    /// <summary>
    /// Resolve whatever the resolution text says.
    /// </summary>
    public class Resolve : IAction
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
    }
}