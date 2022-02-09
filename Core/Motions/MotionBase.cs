namespace Core.Motions
{
    /// <summary>
    /// Things common to all motions.
    /// </summary>
    public abstract class MotionBase : IMotion
    {
        public Person Mover { get; }
        
        protected MotionBase(Person person)
        {
            Mover = person;
        }

        public abstract string GetText();
    }
}