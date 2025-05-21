using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using saga.Models.Entities;

namespace saga.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a base repository for entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity that this repository works with.</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Gets all entities in the repository.
        /// </summary>
        /// <returns>An enumerable collection of all entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to get.</param>
        /// <returns>The entity with the specified ID, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets a single entity by its primary key value, including any related entities specified in the includeProperties parameter.
        /// </summary>
        /// <param name="id">The primary key value of the entity to retrieve.</param>
        /// <param name="includeProperties">An optional array of expressions specifying the related entities to include in the result.</param>
        /// <returns>The entity with the specified primary key value, or null if no matching entity is found.</returns>
        Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="TEntity"/> from the repository.
        /// </summary>
        /// <param name="includeProperties">An array of property expressions to include in the query results.</param>
        /// <returns>An enumerable collection of entities of type <typeparamref name="TEntity"/>.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retrieves a paged subset of entities of type <typeparamref name="TEntity"/> from the repository.
        /// </summary>
        /// <param name="predicate">A predicate expression to filter the entities.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The maximum number of entities per page.</param>
        /// <param name="includeProperties">An array of property expressions to include in the query results.</param>
        /// <returns>An enumerable collection of entities of type <typeparamref name="TEntity"/>.</returns>
        Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Gets a paged collection of entities.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of entities to retrieve per page.</param>
        /// <returns>A paged collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Finds entities that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>A collection of entities that match the predicate.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets a paged collection of entities that match the specified predicate.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of entities to retrieve per page.</param>
        /// <param name="predicate">The predicate to match.</param>
        /// <returns>A paged collection of entities that match the predicate.</returns>
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Adds multiple entities to the repository asynchronously.
        /// </summary>
        /// <param name="entities">The entities to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        Task DeactiveAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity with the specified ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
        Task DeactiveByIdAsync(Guid id);

        /// <summary>
        /// Deletes a range of entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        Task DeactiveRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes a range of entities based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter the entities to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeactiveRangeAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Counts the number of entities that satisfy the specified condition asynchronously.
        /// </summary>
        /// <param name="predicate">The condition that the entities must satisfy.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities that satisfy the specified condition.</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
