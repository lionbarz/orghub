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
    [Route("api/group/{id}/moveresolution")]
    public async Task ActionMoveResolution(string id, [FromBody] MoveResolutionRequest request)
    {
        await _groupService.MoveResolution(Guid.Parse(id), Guid.Parse(request.UserId), request.Text);
    }
    
    [HttpPost]
    [Route("api/group/{id}/action/calltoorder")]
    public async Task ActionCallToOrder(string id, [FromBody] GenericUserRequest request)
    {
        await _groupService.CallToOrder(Guid.Parse(id), Guid.Parse(request.UserId));
    }
}