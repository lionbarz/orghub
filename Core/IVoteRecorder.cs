using System;
using System.Threading.Tasks;

namespace Core
{
    public interface IVoteRecorder
    {
        Task RecordVoteForMotionAsync(Guid groupId, Guid motionId, Guid personId,
            string voteString);
    }
}