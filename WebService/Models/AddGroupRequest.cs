using InterfaceAdapters.Models;

namespace WebService.Models;

/// <summary>
/// Request to create a new group.
/// </summary>
public class AddGroupRequest
{
    /// <summary>
    /// The GUID of the user creating this group. The UI should create the
    /// person first if he doesn't exist and use the existing one if he does.
    /// </summary>
    public string PersonId { get; set; }
    
    /// <summary>
    /// When the new group will have their first meeting.
    /// </summary>
    public UXMeeting NextMeeting { get; set; }
    
    /// <summary>
    /// The name of the group.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The group's stated mission.
    /// </summary>
    public string Mission { get; set; }
}