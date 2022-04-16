using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Motions
{
    public class MotionChain
    {
        public IMotion Current { get; }
        
        /// <summary>
        /// The last is the most recent. New ones are added last.
        /// </summary>
        public IList<IMotion> Previous { get; }

        private MotionChain(IMotion current, IList<IMotion> previous)
        {
            Current = current;
            Previous = previous;
        }
        
        public static MotionChain FromMotion(IMotion motion)
        {
            return new MotionChain(motion, new List<IMotion>());
        }
        
        /// <summary>
        /// Return new chain where it's like the current but:
        /// Drop the current, and make the latest previous the current.
        /// </summary>
        public MotionChain Pop()
        {
            if (!Previous.Any())
            {
                throw new InvalidOperationException("Cannot pop an element because there are no previous motions.");
            }

            return new MotionChain(Previous.Last(), Previous.Take(Previous.Count - 1).ToList());
        }

        public MotionChain Push(IMotion motion)
        {
            var newPrevious = new List<IMotion>();
            newPrevious.AddRange(Previous);
            newPrevious.Add(Current);
            return new MotionChain(motion, newPrevious);
        }
    }
}