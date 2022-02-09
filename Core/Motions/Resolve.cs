using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// Resolve whatever the resolution text says.
    /// </summary>
    public class Resolve : MotionBase
    {
        private string Resolution { get; }

        public Resolve(Person mover, string resolution) : base(mover)
        {
            Resolution = resolution;
        }

        public override string GetText()
        {
            return Resolution;
        }
    }
}