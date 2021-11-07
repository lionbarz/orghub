using System;
using System.Threading.Tasks;

namespace Core
{
    public class MotionToAdjourn : Motion
    {
        private readonly DateTimeOffset _nextMeetingTime;

        public MotionToAdjourn(Person mover, DateTimeOffset nextMeetingTime) : base(mover)
        {
            _nextMeetingTime = nextMeetingTime;
        }

        public override string GetText()
        {
            return $"Adjourn until {_nextMeetingTime}";
        }

        public override Task OnPassage()
        {
            // Nothing.
            return Task.CompletedTask;
        }
    }
}