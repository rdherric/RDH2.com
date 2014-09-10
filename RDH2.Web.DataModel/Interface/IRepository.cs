using System;
using System.Linq;
using System.Linq.Expressions;
using RDH2.Web.DataModel.Context;

namespace RDH2.Web.DataModel.Interface
{
    /// <summary>
    /// IRepository is the interface used to define a repository 
    /// for data access to the RDH2.COM Data Model.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// ModelContext gets the ModelContext that is currently
        /// being used by the Repository.
        /// </summary>
        ModelContext Context { get; }

        
        /// <summary>
        /// GetAll returns the complete list of all of the 
        /// entities in the data store.
        /// </summary>
        /// <returns>List of all entities</returns>
        IQueryable<T> GetAll();


        /// <summary>
        /// GetBy returns the list of entities that satisfy
        /// the filter condition.
        /// </summary>
        /// <param name="filter">The condition to use as the filter</param>
        /// <returns>List of entities that satisfy the condition</returns>
        IQueryable<T> GetBy(Expression<Func<T, Boolean>> filter);


        /// <summary>
        /// Add adds a new entity to the data store without any checks
        /// for uniqueness.
        /// </summary>
        /// <param name="entity">The entity to add to the data store</param>
        /// <returns>New Entity that has been added to the data store</returns>
        T Add(T entity);


        /// <summary>
        /// AddUnique adds a new entity to the data store, first checking
        /// to see that there is not another entity with the unique filter.
        /// </summary>
        /// <param name="entity">The entity to add to the data store</param>
        /// <param name="uniqueFilter">Filter expression to check that the entity is unique</param>
        /// <returns>New Entity that has been added to the data store</returns>
        T AddUnique(T entity, Expression<Func<T, Boolean>> uniqueFilter);


        /// <summary>
        /// Update updates an entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update in the data store</param>
        /// <returns>The updated Entity</returns>
        T Update(T entity);


        /// <summary>
        /// Delete removes an Entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to remove from the data store</param>
        Int32 Delete(T entity);
    }
}
