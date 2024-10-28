namespace PetPortalCore.Models
{
    /// <summary>
    /// Project.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Project identification.
        /// </summary>
        public Guid Id;

        /// <summary>
        /// Project name.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Project description.
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        /// Project owner.
        /// </summary>
        public Guid OwnerId;
    }
}
