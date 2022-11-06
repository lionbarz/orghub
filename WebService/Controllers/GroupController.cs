using Core;
using InterfaceAdapters;
using InterfaceAdapters.Models;
using Microsoft.AspNetCore.Mvc;
using WebService.Models;

namespace WebService.Controllers;

[ApiController]
public class GroupController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupController(GroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    [Route("api/group")]
    public async Task<IEnumerable<UXGroup>> List()
    {
        return await _groupService.GetGroupsAsync();
    }

    [HttpGet]
    [Route("api/group/{id}")]
    public async Task<UXGroup> Get(string id)
    {
        return await _groupService.GetGroupAsync(Guid.Parse(id));
    }
    
    [HttpPost]
    [Route("api/group")]
    public async Task AddGroup([FromBody] GenericUserRequest request)
    {
        await _groupService.AddGroupAsync(Guid.Parse(request.UserId));
    }
    
    /// <summary>
    /// Get all available actions.
    /// </summary>
    [HttpGet]
    [Route("api/group/{id}/action")]
    public async Task<IEnumerable<string>> GetAvailableActions(string id, string userId)
    {
        var actions = await _groupService.GetAvailableActions(Guid.Parse(id), Guid.Parse(userId));
        return actions;
    }

    [HttpPost]
    [Route("api/group/{id}/markattendance")]
    public async Task MarkAttendance(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.MarkAttendance(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/group/{id}/MoveToAdjourn")]
    public async Task ActionMoveToAdjourn(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.MoveToAdjourn(Guid.Parse(id), Guid.Parse(request.UserId));
    }

    [HttpPost]
    [Route("api/group/{id}/moveresolution")]
    public async Task ActionMoveResolution(string id, [FromBody] MoveResolutionRequest request)
    {
        await _groupService.MoveResolution(Guid.Parse(id), Guid.Parse(request.UserId), request.Text);
    }
    
    [HttpPost]
    [Route("api/group/{id}/calltoorder")]
    public async Task ActionCallToOrder(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.CallToOrder(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/group/{id}/DeclareTimeExpired")]
    public async Task ActionDeclareTimeExpired(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.DeclareTimeExpired(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/group/{id}/second")]
    public async Task ActionSecond(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.Second(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/group/{id}/speak")]
    public async Task ActionSpeak(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.Speak(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/group/{id}/vote")]
    public async Task ActionVote(string id, [FromBody] VoteRequest request)
    {
        if (!Enum.TryParse(request.VoteType, true, out VoteType voteType))
        {
            throw new ArgumentException($"{request.VoteType} is not a valid type of vote.");
        }
        
        await _groupService.Vote(Guid.Parse(id), Guid.Parse(request.UserId), voteType);
    }
    
    [HttpPost]
    [Route("api/group/{id}/yield")]
    public async Task ActionYield(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.Yield(Guid.Parse(id), Guid.Parse(request.UserId));
    }
    
    [HttpPost]
    [Route("api/group/{id}/nominateChair")]
    public async Task NominateChair(string id, [FromBody] GenericPersonRequest request)
    {
        await _groupService.NominateChair(Guid.Parse(id), Guid.Parse(request.UserId), Guid.Parse(request.PersonId));
    }
}