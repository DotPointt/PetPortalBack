namespace PetPortalCore.Models
{
    /// <summary>
    /// User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identification.
        /// </summary>
        public Guid Id;

        /// <summary>
        /// User name.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// User email.
        /// </summary>
        public string Email = string.Empty;

        /// <summary>
        /// User password.
        /// </summary>
        public string PasswordHash = string.Empty;
    }
}
