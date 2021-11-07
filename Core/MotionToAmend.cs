using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// When someone moves to amend another motion.
    /// </summary>
    /// <typeparam name="T">The type of motion being amended.</typeparam>
    public class MotionToAmend<T> : Motion where T : Motion
    {
        private T MotionBeingAmended { get; set; }
        private T MotionAsAmended { get; set; }

        public MotionToAmend(Person mover, T motionBeingAmended, T motionAsAmended) : base(mover)
        {
            MotionBeingAmended = motionBeingAmended;
            MotionAsAmended = motionAsAmended;
        }

        public override string GetText()
        {
            return $"Instead of \"{MotionBeingAmended.GetText()}\", make it \"{MotionAsAmended.GetText()}\"";
        }

        public override Task OnPassage()
        {
            // Nothing.
            return Task.CompletedTask;
        }
    }
}