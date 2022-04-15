using Core.Actions;
using Core.Motions;
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
    [Route("group")]
    public async Task<IEnumerable<UXGroup>> List()
    {
        return await _groupService.GetGroupsAsync();
    }

    [HttpGet]
    [Route("group/{id}")]
    public async Task<UXGroup> Get(string id)
    {
        return await _groupService.GetGroupAsync(Guid.Parse(id));
    }
    
    [HttpPost]
    [Route("group")]
    public async Task AddGroup([FromBody] GenericUserRequest request)
    {
        IAction action = new CallMeetingToOrder();
        await _groupService.AddGroupAsync(Guid.Parse(request.UserId));
    }
    
    /// <summary>
    /// Get all available actions.
    /// </summary>
    [HttpGet]
    [Route("group/{id}/action")]
    public async Task<IEnumerable<string>> GetAvailableActions(string id, string userId)
    {
        var actions = await _groupService.GetAvailableActions(Guid.Parse(userId), Guid.Parse(id));
        return actions;
    }
    
    /// <summary>
    /// Get all available motions.
    /// </summary>
    [HttpGet]
    [Route("group/{id}/motion")]
    public async Task<IEnumerable<string>> GetAvailableMotions(string id, string userId)
    {
        var motions = await _groupService.GetAvailableMotions(Guid.Parse(userId), Guid.Parse(id));
        return motions;
    }
    
    [HttpGet]
    [Route("group/{id}/minutes")]
    public async Task<IEnumerable<string>> GetMinutes(string id)
    {
        return await _groupService.GetMinutes(Guid.Parse(id));
    }

    [HttpPost]
    [Route("group/{id}/action/calltoorder")]
    public async Task ActionCallToOrder(string id, [FromBody] GenericUserRequest request)
    {
        IAction action = new CallMeetingToOrder();
        await _groupService.ActAsync(Guid.Parse(request.UserId), Guid.Parse(id), action);
    }

    [HttpPost]
    [Route("group/{id}/action/adjourn")]
    public async Task ActionAdjourn(string id, [FromBody] GenericUserRequest request)
    {
        IAction action = new MoveToAdjourn();
        await _groupService.ActAsync(Guid.Parse(request.UserId), Guid.Parse(id), action);
    }

    [HttpPost]
    [Route("group/{id}/action/declaremotionpassed")]
    public async Task ActionDeclareMotionPassed(string id, [FromBody] GenericUserRequest request)
    {
        IAction action = new DeclareMotionPassed();
        await _groupService.ActAsync(Guid.Parse(request.UserId), Guid.Parse(id), action);
    }

    [HttpPost]
    [Route("group/{id}/action/electchair")]
    public async Task ActionElectChair(string id, [FromBody] GenericPersonRequest request)
    {
        await _groupService.ElectChair(Guid.Parse(request.UserId), Guid.Parse(id), request.PersonId);
    }
    
    [HttpPost]
    [Route("group/{id}/action/movegrantmembership")]
    public async Task ActionMoveGrantMembership(string id, [FromBody] GenericPersonRequest request)
    {
        await _groupService.MoveGrantMembership(Guid.Parse(request.UserId), Guid.Parse(id),
            Guid.Parse(request.PersonId));
    }

    [HttpPost]
    [Route("group/{id}/action/moveresolution")]
    public async Task ActionMoveResolution(string id, [FromBody] MoveResolutionRequest request)
    {
        await _groupService.MoveResolution(Guid.Parse(request.UserId), Guid.Parse(id), request.Text);
    }
    
    [HttpPost]
    [Route("group/{id}/action/movechangegroupname")]
    public async Task ActionMoveChangeGroupName(string id, [FromBody] MoveChangeGroupName request)
    {
        await _groupService.MoveChangeGroupName(Guid.Parse(request.UserId), Guid.Parse(id), request.Name);
    }
    
    [HttpPost]
    [Route("group/{id}/action/second")]
    public async Task ActionSecond(string id, [FromBody] GenericUserRequest request)
    {
        IAction action = new SecondMotion();
        await _groupService.ActAsync(Guid.Parse(request.UserId), Guid.Parse(id), action);
    }
    
    [HttpPost]
    [Route("group/{id}/action/moveenddebate")]
    public async Task ActionMoveEndDebate(string id, [FromBody] GenericUserRequest request)
    {
        IAction action = new Move(new EndDebate());
        await _groupService.ActAsync(Guid.Parse(request.UserId), Guid.Parse(id), action);
    }
    
    [HttpPost]
    [Route("group/{id}/action/speak")]
    public async Task ActionSpeak(string id, [FromBody] GenericUserRequest request)
    {
        IAction action = new Speak();
        await _groupService.ActAsync(Guid.Parse(request.UserId), Guid.Parse(id), action);
    }
    
    [HttpPost]
    [Route("group/{id}/action/yield")]
    public async Task ActionYield(string id, [FromBody] GenericUserRequest request)
    {
        IAction action = new YieldTheFloor();
        await _groupService.ActAsync(Guid.Parse(request.UserId), Guid.Parse(id), action);
    }
}