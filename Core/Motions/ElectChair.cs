using System;
using System.Threading.Tasks;

namespace Core.Motions
{
    /// <summary>
    /// Set the chair of a group to a specific person.
    /// </summary>
    public class ElectChair : GroupModifyingMotion, IMainMotion
    {
        private readonly Person _nominee;

        public ElectChair(Person nominee, IGroupModifier groupModifier) : base(groupModifier)
        {
            _nominee = nominee;
        }
        
        public override string GetText()
        {
            return $"Elect {_nominee.Name} as the chair of the group.";
        }

        public override Task TakeActionAsync()
        {
            GroupModifier.SetChair(_nominee);
            GroupModifier.AddResolution(GetText());
            return Task.CompletedTask;
        }
    }
}