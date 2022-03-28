namespace Core.Actions
{
    /// <summary>
    /// What roles something is available to.
    /// </summary>
    public class Availability
    {
        public bool IsAvailableToChairs { get; init; }
        public bool IsAvailableToMembers { get; init; }
        public bool IsAvailableToGuests { get; init; }

        public Availability(bool guests, bool members, bool chairs)
        {
            IsAvailableToGuests = guests;
            IsAvailableToMembers = members;
            IsAvailableToChairs = chairs;
        }
    }
}