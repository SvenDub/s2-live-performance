namespace Live_Performance.Persistence.Exception
{
    /// <summary>
        ///     Thrown when an entity does not have its Identity member set when it should have.
        /// </summary>
    public class EntityWithoutIdentityException : EntityException
    {
        public EntityWithoutIdentityException() : base("The entity is expected to have its Identity member set, but has not.")
        {
        }

        public EntityWithoutIdentityException(string message) : base(message)
        {
        }

        public EntityWithoutIdentityException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}