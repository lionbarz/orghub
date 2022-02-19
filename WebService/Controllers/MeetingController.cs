using Core.Actions;
using InterfaceAdapters;
using InterfaceAdapters.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers;

[ApiController]
[Route("[controller]")]
public class MeetingController : ControllerBase
{
    private readonly MeetingService _meetingService;
    public static Guid _userId = MeetingService._userId;
    
    public MeetingController(MeetingService meetingService)
    {
        _meetingService = meetingService;
    }
    
    [HttpGet]
    public async Task<IEnumerable<UXMeeting>> Get()
    {
        var meetings = await _meetingService.GetMeetingsAsync();

        if (!meetings.Any())
        {
            await _meetingService.AddMeeting(new UXMeeting());
            meetings = await _meetingService.GetMeetingsAsync();
        }

        return meetings;
    }

    public class ActionRequest {
        public string MeetingIdString { get; set; }
        public string ActionName { get; set; }
    }
    
    [HttpPost]
    public async Task Post([FromBody] ActionRequest request)
    {
        string actionName = request.ActionName;
        string meetingIdString = request.MeetingIdString;
        
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
    /*
    [HttpPost]
    [Route("meeting/start")]
    public async Task Start()
    {
        var meetingId = Guid.NewGuid();
        var action = new CallMeetingToOrder();
        await _meetingService.TakeActionAsync(meetingId, _userId, action);
    }
    
    [HttpPost]
    [Route("meeting/start")]
    public async Task Start(string meetingIdString)
    {
        var meetingId = Guid.Parse(meetingIdString);
        var action = new CallMeetingToOrder();
        await _meetingService.TakeActionAsync(meetingId, _userId, action);
    }

    [HttpPost]
    public async Task Speak(string meetingIdString)
    {
        var meetingId = Guid.Parse(meetingIdString);
        var action = new Speak();
        await _meetingService.TakeActionAsync(meetingId, _userId, action);
    }
    
    [HttpPost]
    public async Task Yield(string meetingIdString)
    {
        var meetingId = Guid.Parse(meetingIdString);
        var action = new YieldTheFloor();
        await _meetingService.TakeActionAsync(meetingId, _userId, action);
    }
*/
}