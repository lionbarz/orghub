namespace WebService.Models;

public class GenericUserRequest
{
    /// <summary>
    /// The logged in user who is initiating the action.
    /// </summary>
    public string? UserId { get; set; }
}