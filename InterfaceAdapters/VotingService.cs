using System;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace InterfaceAdapters
{
    /// <summary>
    /// Voting service saves and reads a vote from storage.
    /// </summary>
    public class VotingService : IVoteRecorder
    {
        private readonly IDatabaseAccess _database;
        public VotingService(IDatabaseAccess database)
        {
            _database = database;
        }

        public async Task<Vote> GetVote(Guid groupId, Guid motionId)
        {
            Group group = await _database.GetGroupAsync(groupId);
            var motion = group.Motions.FirstOrDefault(x => x.Id == motionId);
            return motion.Vote;
        }
        
        /// <summary>
        /// Starts a vote.
        /// </summary>
        public async Task<Vote> StartVoteOnMotionAsync(Guid groupId, Guid motionId)
        {
            Group group = await _database.GetGroupAsync(groupId);
            var motion = group.Motions.FirstOrDefault(x => x.Id == motionId);

            if (motion == null)
            {
                throw new Exception($"No motion exists with Id {motionId}");
            }
            
            Vote vote = new Vote(group.Members);
            motion.Vote = vote;
            return vote;
        }

        /// <summary>
        /// Records the vote of a person for a given vote.
        /// </summary>
        public async Task RecordVoteForMotionAsync(Guid groupId, Guid motionId, Guid personId,
            string voteString)
        {
            Group group = await _database.GetGroupAsync(groupId);
            var motion = group.Motions.FirstOrDefault(x => x.Id == motionId);
            Person person = await _database.GetPersonAsync(personId);

            var voteType = VoteType.Abstain;

            if (Enum.TryParse(voteString, true, out VoteType voterVote))
            {
                voteType = voterVote;
            }

            motion.Vote.RecordVote(person, voteType);
        }
    }
}