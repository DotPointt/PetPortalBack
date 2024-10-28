namespace PetPortalCore.Models
{
    /// <summary>
    /// Project.
    /// </summary>
    public class Project
    {
        public const int MAX_NAME_LENGHT = 250;
        public Project(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

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

        public static (Project project, string Error) Create(Guid id, string name, string description)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGHT)
            {
                error = "Name can not be empty or longer then 250 symbols";
            }

            var project = new Project(id, name, description);

            return (project, error);
        }
    }


}
