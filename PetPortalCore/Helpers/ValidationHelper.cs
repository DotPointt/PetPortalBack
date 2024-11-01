namespace PetPortalCore.Helpers
{
    /// <summary>
    /// Validation helper. 
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// Check guid is non empty.
        /// </summary>
        /// <param name="id">Guid to check.</param>
        /// <exception cref="ArgumentException">If guid is empty.</exception>
        public void CheckGuidIsNonEmpty(Guid id)
        {
            if (Guid.Empty == id)
            {
                throw new ArgumentException("Guid is empty.", nameof(id));
            }
        }

        /// <summary>
        /// Check string is non null or empty.
        /// </summary>
        /// <param name="checkedString">Checked string.</param>
        /// <exception cref="ArgumentException">If string is null or empty.</exception>
        public void CheckStringIsNonEmpty(string checkedString)
        {
            if (string.IsNullOrEmpty(checkedString))
            {
                throw new ArgumentException("String is null or empty.", nameof(checkedString));
            }
        }

        /// <summary>
        /// check object is non null.
        /// </summary>
        /// <param name="checkedObject">Checked object.</param>
        /// <exception cref="NullReferenceException">if object is null.</exception>
        public void CheckObjectIsNonNull(object? checkedObject)
        {
            if (checkedObject is null)
            {
                throw new NullReferenceException($"{checkedObject} is null.");
            }
        }
    }
}