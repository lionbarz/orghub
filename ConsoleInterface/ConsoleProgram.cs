using System;
using System.Threading.Tasks;
using Core;
using InterfaceAdapters;

namespace ConsoleInterface
{
    public class ConsoleProgram
    {
        private VotingService VotingService { get; set; }

        private MotionService MotionService { get; set; }
        
        private GroupService GroupService { get; set; }
        
        private PersonService PersonService { get; set; }

        public ConsoleProgram(VotingService votingService, MotionService motionService, GroupService groupService, PersonService personService)
        {
            VotingService = votingService;
            MotionService = motionService;
            GroupService = groupService;
            PersonService = personService;
        }

        public async Task RunAsync()
        {
            // The UI gathers all the data in its own way. In this instance it's the console.
            await MoveToCreateOrganizationAsync();
        }

        public async Task MoveToCreateOrganizationAsync()
        {
            Group group = new Group();
            Person mo = new()
            {
                // TODO: Have the service assign the guid.
                Id = Guid.NewGuid(),
                Name = "Mohamed Fakhreddine",
                Email = "mohamed.y.fakhreddine@gmail.com"
            };
            Person roni = new()
            {
                Id = Guid.NewGuid(),
                Name = "Rawan Hammoud",
                Email = "rawan.hammoud@gmail.com"
            };
            group.AddMember(mo);
            group.AddMember(roni);
            await PersonService.AddPerson(mo);
            await PersonService.AddPerson(roni);
            await GroupService.AddGroupAsync(group);
            
            Console.WriteLine("What is the proposed organization name?");
            string organizationName = Console.ReadLine();
            Console.WriteLine("What is the proposed organization mission?");
            string mission = Console.ReadLine();
            Bylaws bylaws = new Bylaws()
            {
                Name = organizationName,
                Mission = mission
            };

            MotionToCreateOrganization motion = new MotionToCreateOrganization(mo, bylaws);
            await MotionService.MakeMotionAsync(group.Id, motion);

            Console.WriteLine("What should the new mission be?");
            var newMission = Console.ReadLine();
            MotionToCreateOrganization amendedMotion = new MotionToCreateOrganization(mo, bylaws);
            var amendMotion = new MotionToAmend<MotionToCreateOrganization>(roni, motion,
                new MotionToCreateOrganization(roni,
                    new Bylaws() { Name = organizationName, Mission = newMission }));
            await MotionService.MakeMotionAsync(group.Id, amendMotion);
            
            var vote = await VotingService.StartVoteOnMotionAsync(group.Id, amendMotion.Id);
            foreach (var voter in vote.EligibleVoters)
            {
                var voteText = GetVote(voter);
                await VotingService.RecordVoteForMotionAsync(group.Id, amendMotion.Id, voter.Id, voteText);

            }
            
            vote = await VotingService.GetVote(group.Id, amendMotion.Id);
            Console.WriteLine($"Vote for {amendMotion.GetText()} is {vote.Result}");
        }

        private string GetVote(Person person)
        {
            Console.WriteLine($"How does {person.Name} vote?");
            return Console.ReadLine();
        }
    }
}