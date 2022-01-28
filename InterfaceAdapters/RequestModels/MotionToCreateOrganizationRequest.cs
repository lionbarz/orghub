using System;

namespace InterfaceAdapters.RequestModels
{
    /// <summary>
    /// A request to create a motion for a group.
    /// </summary>
    public class MotionToCreateOrganizationRequest
    {
        /// <summary>
        /// The group the person is making the request to.
        /// </summary>
        public Guid GroupId { get; set; }
        
        /// <summary>
        /// The person making the request.
        /// </summary>
        public Guid PersonId { get; set; }
        
        /// <summary>
        /// The bylaws of the created organization.
        /// </summary>
        public Bylaws Bylaws { get; set; }
    }
}