using System;
using System.Collections.Generic;
using Live_Performance.Persistence.Exception;

namespace Live_Performance.Persistence
{
    /// <summary>
    ///     Interface for generic CRUD operations on a repository for a specified type.
    /// </summary>
    /// <typeparam name="T">The entity type the repository manages.</typeparam>
    public interface IRepository<T> where T : new()
    {
        /// <summary>
        ///     Returns the number of entities available
        /// </summary>
        /// <returns>The number of entities.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        long Count();

        /// <summary>
        ///     Deletes the entity with the given id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        void Delete(int id);

        /// <summary>
        ///     Deletes the given entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        void Delete(List<T> entities);

        /// <summary>
        ///     Deletes a given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        void Delete(T entity);

        /// <summary>
        ///     Deletes all entities managed by the repository.
        /// </summary>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        void DeleteAll();

        /// <summary>
        ///     Returns whether an entity with the given id exists.
        /// </summary>
        /// <param name="id">The entity to check.</param>
        /// <returns>True if an entity with the given id exists, false otherwise.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        bool Exists(int id);

        /// <summary>
        ///     Returns all instances of the type.
        /// </summary>
        /// <returns>All entities.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        List<T> FindAll();

        /// <summary>
        ///     Returns all instances of the type with the given IDs.
        /// </summary>
        /// <param name="ids">The IDs to check.</param>
        /// <returns>All entities with the given IDs.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        List<T> FindAll(List<int> ids);

        /// <summary>
        ///     Returns all instances of the type that satisfy the given predicate.
        /// </summary>
        /// <param name="predicate">The condition to check.</param>
        /// <returns>All entities that satisfy the given predicate.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        List<T> FindAllWhere(Func<T, bool> predicate);

        /// <summary>
        ///     Returns all instances of the type that satisfy the given predicate.
        /// </summary>
        /// <param name="predicate">The condition to check.</param>
        /// <returns>All entities that satisfy the given predicate.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        List<T> FindAllWhere(Func<T, int, bool> predicate);

        /// <summary>
        ///     Retrieves an entity by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The entity with the given id or null if none found.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        T FindOne(int id);

        /// <summary>
        ///     Saves a given entity.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <returns>The saved entity.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        T Save(T entity);

        /// <summary>
        ///     Saves all given entities.
        /// </summary>
        /// <param name="entities">The entities to save.</param>
        /// <returns>The saved entities.</returns>
        /// <exception cref="ConnectException">When the connection to the data source failed.</exception>
        /// <exception cref="DataSourceException">When an error occured while querying or reading from the data source.</exception>
        /// <exception cref="EntityException">When an entity does not match its data representation.</exception>
        /// <exception cref="EntityNotFoundException">When a query returns no results.</exception>
        List<T> Save(List<T> entities);
    }
}