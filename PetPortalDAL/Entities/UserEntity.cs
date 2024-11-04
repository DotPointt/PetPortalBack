namespace PetPortalDAL.Entities
{
    /// <summary>
    /// User as data base model.
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        public string PasswordHash { get; set; }
    }
}