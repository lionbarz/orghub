using System;
using System.Threading.Tasks;

namespace Core.Actions
{
    public class Adjourn : IAction
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