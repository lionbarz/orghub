using Core;

namespace WebService.Models;

public class VoteRequest : GenericUserRequest
{
    /// <summary>
    /// String representation of the VoteType enum.
    /// </summary>
    public string VoteType { get; set; }
}