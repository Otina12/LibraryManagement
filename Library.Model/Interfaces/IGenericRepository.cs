using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Model.Interfaces
{
    /// <summary>
    /// Interface for a generic repository providing CRUD and other helpful operations for entities of type T.
    /// </summary>
    /// <typeparam name="T">Type of entity managed by the repository.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Creates a new entity in the repository. The entity is added to the database only after calling SaveChanges.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        Task Create(T entity);

        Task CreateRange(IEnumerable<T> entites);

        /// <summary>
        /// Marks an existing entity as modified. Changes will be applied to the database upon calling SaveChanges.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Marks an existing entity for deletion. The entity will be removed from the database after calling SaveChanges.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Marks all entities matching the specified condition for deletion. Entities will be removed from the database after calling SaveChanges.
        /// </summary>
        /// <param name="where">A predicate to filter entities for deletion.</param>
        void DeleteAllWhere(Expression<Func<T, bool>> where);

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="trackChanges">Indicates whether to track changes of the retrieved entity. Default is false.</param>
        /// <returns>The entity if found, otherwise null.</returns>
        Task<T?> GetById(Guid id, bool trackChanges = false);

        /// <summary>
        /// Retrieves the first entity matching the specified condition.
        /// </summary>
        /// <param name="where">A predicate to filter entities.</param>
        /// <param name="trackChanges">Indicates whether to track changes of the retrieved entity. Default is false.</param>
        /// <returns>The first matching entity if found, otherwise null.</returns>
        Task<T?> GetOneWhere(Expression<Func<T, bool>> where, bool trackChanges = false);

        /// <summary>
        /// Retrieves all entities as a queryable collection. Useful for further filtering or querying operations.
        /// </summary>
        /// <param name="trackChanges">Indicates whether to track changes of the retrieved entities. Default is false.</param>
        /// <returns>An IQueryable collection of entities.</returns>
        IQueryable<T> GetAllAsQueryable(bool trackChanges = false);

        /// <summary>
        /// Retrieves all entities as an enumerable collection.
        /// </summary>
        /// <param name="trackChanges">Indicates whether to track changes of the retrieved entities. Default is false.</param>
        /// <returns>A collection of entities as an IEnumerable.</returns>
        Task<IEnumerable<T>> GetAll(bool trackChanges = false);

        /// <summary>
        /// Retrieves all entities matching the specified condition.
        /// </summary>
        /// <param name="where">A predicate to filter entities.</param>
        /// <param name="trackChanges">Indicates whether to track changes of the retrieved entities. Default is false.</param>
        /// <returns>A collection of entities matching the condition.</returns>
        Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> where, bool trackChanges = false);
    }
}
