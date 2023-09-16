using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Core.Meetings
{
    /// <summary>
    /// Tracks a series of states, which constitute an agenda, and tracks
    /// which is the current item.
    /// </summary>
    /// <remarks>
    /// Upon creation, the current item is null. It needs to be advanced,
    /// like an iterator.
    /// </remarks>
    public class MeetingAgenda
    {
        /// <summary>
        /// The current agenda item, as a meeting state.
        /// </summary>
        public IAgendaItem? GetCurrentItem()
        {
            return (ItemIndex < 0 || ItemIndex >= AllItems.Count) ? null : AllItems[ItemIndex];
        }

        /// <summary>
        /// Moves to the next agenda item.
        /// </summary>
        /// <returns>True if there was one, false otherwise.</returns>
        public bool MoveToNextItem([NotNullWhen(true)] out IAgendaItem? nextItem)
        {
            // Already consumed all agenda items.
            if (ItemIndex >= AllItems.Count)
            {
                nextItem = null;
                return false;
            }

            if (ItemIndex >= 0)
            {
                // There is a current item which is no longer current so update the flag.
                AllItems[ItemIndex].IsCurrent = false;
            }

            // Advance to the next item.
            ItemIndex++;

            // We're beyond the end of the list, ie no more items.
            if (ItemIndex >= AllItems.Count)
            {
                nextItem = null;
                return false;
            }
            
            // Set the new agenda item to current and return it.
            AllItems[ItemIndex].IsCurrent = true;
            nextItem = AllItems[ItemIndex];
            return true;
        }

        /// <summary>
        /// Get the titles of all the agenda items.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAgendaItem> GetAllItems()
        {
            return AllItems;
        }
        
        // All items on the agenda.
        private List<IAgendaItem> AllItems { get; set; }

        // The item that we're currently at.
        private int ItemIndex { get; set; }

        // Constructor
        private MeetingAgenda(IEnumerable<IAgendaItem> allItems)
        {
            ItemIndex = -1; // Start before the end of the list.
            AllItems = allItems.ToList();
            // TODO: I don't like this side effect very much.
            AllItems.ForEach(x => x.IsCurrent = false);
        }

        /// <summary>
        /// Creates an agenda from the given meeting states.
        /// </summary>
        public static MeetingAgenda FromItems(IEnumerable<IAgendaItem> items)
        {
            var list = new LinkedList<IAgendaItem>();
            
            foreach (var item in items)
            {
                list.AddLast(item);
            }

            return new MeetingAgenda(list);
        }

        /// <summary>
        /// Creates an agenda with no items.
        /// </summary>
        /// <returns></returns>
        public static MeetingAgenda EmptyAgenda()
        {
            return new MeetingAgenda(new List<IAgendaItem>());
        }
    }
}