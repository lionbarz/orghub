using Core.Actions;
using InterfaceAdapters;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers;

[ApiController]
[Route("[controller]")]
public class ActionController : ControllerBase
{
    private readonly MeetingService _meetingService;
    private static Guid _userId = Guid.NewGuid();
    
    public ActionController(MeetingService meetingService)
    {
        _meetingService = meetingService;
    }
    
    [HttpPost]
    public async Task Post(string meetingIdString, string actionName)
    {
        IAction? action = null;

        if (actionName == "start")
        {
            action = new CallMeetingToOrder();
        } else if (actionName == "speak")
        {
            action = new Speak();
        } else if (actionName == "yield")
        {
            action = new YieldTheFloor();
        }

        if (action == null)
        {
            throw new ArgumentException($"{actionName} is not a valid action");
        }
        
        var meetingId = Guid.Parse(meetingIdString);
        await _meetingService.TakeActionAsync(meetingId, _userId, action);
    }
    
    
}