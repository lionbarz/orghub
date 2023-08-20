using Core;
using InterfaceAdapters;
using InterfaceAdapters.Models;
using Microsoft.AspNetCore.Mvc;
using WebService.Models;

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
    [Route("api/meeting")]
    public async Task<IEnumerable<UXMeeting>> List()
    {
        return await MeetingService.GetMeetingsAsync();
    }
    
    [HttpGet]
    [Route("api/meeting/{id}")]
    public async Task<UXMeeting> Get(string id)
    {
        return await MeetingService.GetMeetingAsync(Guid.Parse(id));
    }
    
    /// <summary>
    /// Create a new mass meeting.
    /// </summary>
    [HttpPost]
    [Route("api/meeting/{id}/action")]
    public async Task<UXMeeting> CreateMassMeeting(string userId)
    {
        return await MeetingService.CreateMassMeetingAsync(Guid.Parse(userId), DateTimeOffset.Now, "", "");
    }
    
    /// <summary>
    /// Get all available actions.
    /// </summary>
    [HttpGet]
    [Route("api/meeting/{id}/action")]
    public async Task<IEnumerable<string>> GetAvailableActions(string id, string userId)
    {
        var actions = await MeetingService.GetAvailableActions(Guid.Parse(id), Guid.Parse(userId));
        return actions;
    }

    [HttpPost]
    [Route("api/meeting/{id}/markattendance")]
    public async Task MarkAttendance(string id, [FromBody] GenericUserRequest request)
    {
        await MeetingService.MarkAttendance(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/MoveToAdjourn")]
    public async Task ActionMoveToAdjourn(string id, [FromBody] GenericUserRequest request)
    {
        await MeetingService.MoveToAdjourn(Guid.Parse(id), Guid.Parse(request.UserId));
    }

    [HttpPost]
    [Route("api/meeting/{id}/moveresolution")]
    public async Task ActionMoveResolution(string id, [FromBody] MoveResolutionRequest request)
    {
        await MeetingService.MoveResolution(Guid.Parse(id), Guid.Parse(request.UserId), request.Text);
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/calltoorder")]
    public async Task ActionCallToOrder(string id, [FromBody] GenericUserRequest request)
    {
        await MeetingService.CallToOrder(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/DeclareTimeExpired")]
    public async Task ActionDeclareTimeExpired(string id, [FromBody] GenericUserRequest request)
    {
        await MeetingService.DeclareTimeExpired(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/second")]
    public async Task ActionSecond(string id, [FromBody] GenericUserRequest request)
    {
        await MeetingService.Second(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/speak")]
    public async Task ActionSpeak(string id, [FromBody] GenericUserRequest request)
    {
        await MeetingService.Speak(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/vote")]
    public async Task ActionVote(string id, [FromBody] VoteRequest request)
    {
        if (!Enum.TryParse(request.VoteType, true, out VoteType voteType))
        {
            throw new ArgumentException($"{request.VoteType} is not a valid type of vote.");
        }
        
        await MeetingService.Vote(Guid.Parse(id), Guid.Parse(request.UserId), voteType);
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/yield")]
    public async Task ActionYield(string id, [FromBody] GenericUserRequest request)
    {
        await MeetingService.Yield(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/meeting/{id}/nominateChair")]
    public async Task NominateChair(string id, [FromBody] GenericPersonRequest request)
    {
        await MeetingService.NominateChair(Guid.Parse(id), Guid.Parse(request.UserId), Guid.Parse(request.PersonId));
    }
}