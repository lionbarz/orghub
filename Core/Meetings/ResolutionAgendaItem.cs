
namespace Core.Meetings
{
    /// <summary>
    /// An item on the agenda that proposes a new resolution.
    /// </summary>
    public class ResolutionAgendaItem : IAgendaItem
    {
        public string Text { get; }
        
        public Person Sponsor { get; }

        public bool IsCurrent { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sponsor"></param>
        /// <param name="text">
        /// TODO: Make this a complex object and reuse it in the Move motion.
        /// </param>
        public ResolutionAgendaItem(Person sponsor, string text)
        {
            Text = text;
            Sponsor = sponsor;
        }
        
        public string GetTitle()
        {
            return $"Resolution by {Sponsor.Name}: {Text}";
        }
    }
}