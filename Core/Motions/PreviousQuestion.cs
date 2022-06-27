using Core.Actions;

namespace Core.Motions
{
    /// <summary>
    /// "The motion for the previous question is used during the consideration of
    /// a matter to terminate debate, foreclose the offering of amendments, and
    /// to bring the House to an immediate vote on the main question."
    /// https://www.govinfo.gov/content/pkg/GPO-HPRACTICE-104/pdf/GPO-HPRACTICE-104-40.pdf
    /// </summary>
    public class PreviousQuestion : ISubsidiaryMotion
    {
        public string GetText()
        {
            return $"Stop debating and vote.";
        }
    }
}