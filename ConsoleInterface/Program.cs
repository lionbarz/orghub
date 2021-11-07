using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Databases;
using InterfaceAdapters;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        static async Task Run()
        {
            IDatabaseAccess db = new InMemoryDatabase();
            VotingService votingService = new VotingService(db);
            MotionService motionService = new MotionService(db);
            GroupService groupService = new(db);
            PersonService personService = new(db);
            ConsoleProgram program = new ConsoleProgram(votingService, motionService, groupService, personService);
            await program.RunAsync();
        }
    }
}