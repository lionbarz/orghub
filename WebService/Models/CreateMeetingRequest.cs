namespace WebService.Models;

public class CreateMeetingRequest : GenericUserRequest
{
    public string? GroupId { get; set; }

    public DateTimeOffset MeetingStartTime { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }
}