namespace PetPortalCore.DTOs
{
    /// <summary>
    /// Detail project dto.
    /// </summary>
    public class ProjectDetailDto
    {
        /// <summary>
        /// Project identifier.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Project description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Project owner identifier.
        /// </summary>
        public Guid OwnerId { get; set; }
    }
}