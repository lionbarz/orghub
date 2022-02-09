using System;
using System.Threading.Tasks;

namespace Core.Motions
{
    public class Adjourn : MotionBase
    {
        private readonly DateTimeOffset _nextMeetingTime;

        public Adjourn(Person mover, DateTimeOffset nextMeetingTime) : base(mover)
        {
            _nextMeetingTime = nextMeetingTime;
        }

        public override string GetText()
        {
            return $"Adjourn until {_nextMeetingTime}";
        }
    }
}