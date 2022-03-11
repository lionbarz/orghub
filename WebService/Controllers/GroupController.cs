using Core.Actions;
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
    public async Task<IEnumerable<UXGroup>> Get()
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
    [Route("group/{id}/action/calltoorder")]
    public async Task ActionCallToOrder(string id)
    {
        IAction action = new CallMeetingToOrder();
        await _groupService.ActAsync("blah", Guid.Parse(id), action);
    }

    [HttpPost]
    [Route("group/{id}/action/adjourn")]
    public async Task ActionAdjourn(string id)
    {
        IAction action = new MoveToAdjourn();
        await _groupService.ActAsync("blah", Guid.Parse(id), action);
    }

    [HttpPost]
    [Route("group/{id}/action/declaremotionpassed")]
    public async Task ActionDeclareMotionPassed(string id)
    {
        IAction action = new DeclareMotionPassed();
        await _groupService.ActAsync("blah", Guid.Parse(id), action);
    }

    [HttpPost]
    [Route("group/{id}/action/electchair")]
    public async Task ActionElectChair(string id, [FromBody] ElectChairRequest request)
    {
        await _groupService.ElectChair(Guid.Parse(id), request.NomineeName);
    }

    [HttpPost]
    [Route("group/{id}/action/moveresolution")]
    public async Task ActionMoveResolution(string id, [FromBody] MoveResolutionRequest request)
    {
        await _groupService.MoveResolution(Guid.Parse(id), request.Text);
    }
}