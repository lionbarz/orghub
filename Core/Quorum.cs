using System;

namespace Core
{
    /// <summary>
    /// Represents a quorum requirement.
    ///
    /// It can either be:
    /// - An absolute minimum number of people.
    /// - More than some percentage (ie, > 50% or simple majority).
    /// </summary>
    public class Quorum
    {
        /// <summary>
        /// The number, if number based.
        /// </summary>
        private int Absolute { get; set; }
        
        /// <summary>
        /// The percentage, if percentage based.
        /// </summary>
        private float Percentage { get; set;  }

        /// <summary>
        /// True if the calculation uses percentages.
        /// False if it uses absolute numbers.
        /// </summary>
        private bool IsPercentageBased { get; set;  }
        
        /// <summary>
        /// If false, the number must just be met.
        /// If true, it must be greater than the number.
        /// </summary>
        private bool MustBeGreater { get; set; }

        public static Quorum SimpleMajority()
        {
            return new Quorum()
            {
                IsPercentageBased = true,
                Percentage = 0.5f,
                MustBeGreater = true
            };
        }
        
        /// <summary>
        /// How many need to be present.
        /// </summary>
        /// <param name="membershipSize">How many in the membership.</param>
        /// <returns>Quorum</returns>
        public int GetQuorumNumber(int membershipSize)
        {
            if (IsPercentageBased)
            {
                float required = membershipSize / Percentage;

                if (MustBeGreater)
                {
                    if ((int)required == required)
                    {
                        return (int)required + 1;
                    }
                    else
                    {
                        return (int)Math.Ceiling(required);
                    }
                }
                else
                {
                    return (int)Math.Ceiling(required);
                }
            }
            else
            {
                return MustBeGreater ? Absolute + 1 : Absolute;
            }
        }
    }
}