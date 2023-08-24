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
    public async Task<UXGroup> AddGroup([FromBody] AddGroupRequest request)
    {
        return await _groupService.AddGroupAsync(
            Guid.Parse(request.PersonId),
            request.Name,
            request.Mission,
            request.NextMeeting,
            request.MemberEmails);
    }
}