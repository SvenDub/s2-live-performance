namespace Live_Performance.Persistence
{
    /// <summary>
    ///     Actual implementation of <see cref="IRepository{T}"/> protected by <see cref="RepositoryArmour{T}"/>.
    /// </summary>
    /// <typeparam name="T">The entity type the repository manages.</typeparam>
    /// <seealso cref="IRepository{T}"/>
    /// <seealso cref="RepositoryArmour{T}"/>
    public interface IStrictRepository<T> : IRepository<T> where T : new()
    {
    }
}