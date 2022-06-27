using InterfaceAdapters;
using InterfaceAdapters.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers;

[ApiController]
public class MeetingController : ControllerBase
{
    private MeetingService MeetingService { get; }

    public MeetingController(MeetingService meetingService)
    {
        MeetingService = meetingService;
    }
    
    [HttpGet]
    [Route("meeting")]
    public async Task<IEnumerable<UXMeeting>> List()
    {
        return await MeetingService.GetMeetingsAsync();
    }
    
    [HttpGet]
    [Route("meeting/{id}")]
    public async Task<UXMeeting> Get(string id)
    {
        return await MeetingService.GetMeetingAsync(Guid.Parse(id));
    }
    
    /// <summary>
    /// Create a new mass meeting.
    /// </summary>
    [HttpPost]
    [Route("meeting/{id}/action")]
    public async Task<UXMeeting> CreateMassMeeting(string userId)
    {
        return await MeetingService.CreateMassMeetingAsync(Guid.Parse(userId), DateTimeOffset.Now);
    }
}