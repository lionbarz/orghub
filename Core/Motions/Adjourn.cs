using System;
using System.Threading.Tasks;

namespace Core.Motions
{
    public class Adjourn : IMotion
    {
        private readonly DateTimeOffset _nextMeetingTime;

        public Adjourn(DateTimeOffset nextMeetingTime)
        {
            _nextMeetingTime = nextMeetingTime;
        }

        public string GetText()
        {
            return $"Adjourn until {_nextMeetingTime}";
        }
    }
}