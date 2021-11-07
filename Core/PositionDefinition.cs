namespace Core
{
    /// <summary>
    /// This is a position that can be occupied by a single person.
    /// </summary>
    public class PositionDefinition
    {
        /// <summary>
        /// The name of the position. Ex: President, Secretary, member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// What is the minimum number of instances of this position?
        /// </summary>
        public int MinimumInstances { get; set; }
        
        /// <summary>
        /// What is the maximum number of instances of this position?
        /// </summary>
        public int MaximumInstances { get; set; }
    }
}