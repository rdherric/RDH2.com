using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RDH2.Web.DataModel.Context;
using RDH2.Web.DataModel.Interface;

namespace RDH2.Web.DataModel.Model
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        #region Member Variables
        private readonly ModelContext _context = null;
        #endregion

        
        #region Constructors
        /// <summary>
        /// Default Constructor for the Repository object.
        /// </summary>
        public Repository()
        {
            //Create the ModelContext from scratch
            this._context = ModelContext.Current;
        }


        /// <summary>
        /// Constructor that takes a Connection String to connect
        /// to the data store.
        /// </summary>
        /// <param name="connectionString">The Connection String used to connect to the database</param>
        public Repository(String connectionString)
        {
            //Save the input in the Member Variables
            this._context = new ModelContext(connectionString);
        }


        /// <summary>
        /// Constructor that takes a ModelContext to hold the
        /// DbContext if necessary.
        /// </summary>
        /// <param name="context">The ModelContext to use for query and saving</param>
        public Repository(ModelContext context)
        {
            //Save the input in the member variables
            this._context = context;
        }
        #endregion


        #region IRepository Implementation
        /// <summary>
        /// Context gets the ModelContext currently being used by 
        /// this Repository.
        /// </summary>
        public virtual ModelContext Context
        {
            get { return this._context; }
        }


        /// <summary>
        /// GetAll gets the complete List of the entities.
        /// </summary>
        /// <returns>IQueryable list of the entities</returns>
        public virtual IQueryable<T> GetAll()
        {
            //Return the DbSet as an IQueryable
            return this._context.Set<T>().AsQueryable();
        }


        /// <summary>
        /// GetBy retrieves a List of entities filtered by the 
        /// specified Expression.
        /// </summary>
        /// <param name="filter">The Expression to use to filter the entities</param>
        /// <returns>IQueryable list of the filtered entities</returns>
        public virtual IQueryable<T> GetBy(Expression<Func<T, Boolean>> filter)
        {
            //Return the DbSet as an IQueryable with the filter applied
            return this._context.Set<T>().Where(filter).AsQueryable();
        }


        /// <summary>
        /// Add adds a new entity to the data store without any 
        /// unique check.
        /// </summary>
        /// <param name="entity">The entity to add to the data store</param>
        /// <returns>The newly added entity</returns>
        public T Add(T entity)
        {
            //Define a variable to return
            T rtn = this._context.Set<T>().Add(entity);

            //Save the changes to the database
            this._context.SaveChanges();

            //Now Reload the data from the database in case there
            //are derived values
            this._context.Entry<T>(entity).Reload();

            //Return the result
            return rtn;
        }


        /// <summary>
        /// AddUnique adds a new Entity to the data store, first
        /// checking for the entity by the unique filter.
        /// </summary>
        /// <param name="entity">The entity to add to the data store</param>
        /// <returns>The newly added entity</returns>
        public virtual T AddUnique(T entity, Expression<Func<T, Boolean>> uniqueFilter)
        {
            //Define a variable to return
            T rtn = entity;
            
            //Try to find the entity using the unique filter -- if it
            //isn't found, add the new entity
            if (this._context.Set<T>().Where(uniqueFilter).SingleOrDefault() == null)
            {
                //Add the new entity
                rtn = this.Add(entity);
            }
            else
            {
                throw new InvalidOperationException("Non-unique value was submitted to the database");
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Update updates an entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity</returns>
        public virtual T Update(T entity)
        {
            //Set the values to Modified
            this._context.Entry<T>(entity).State = EntityState.Modified;

            //Save the changes
            this._context.SaveChanges();

            //Now Reload the data from the database in case there
            //are derived values
            this._context.Entry<T>(entity).Reload();

            //Return the result
            return entity;
        }


        /// <summary>
        /// Delete removes an entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to remove from the data store</param>
        /// <returns>Int32 to show affected rows</returns>
        public virtual Int32 Delete(T entity)
        {
            //Delete the object from the data store
            this._context.Set<T>().Remove(entity);

            //Save the changes to the database
            return this._context.SaveChanges();
        }
        #endregion


        #region IDisposable Implementation
        /// <summary>
        /// Dispose disposes of the ModelContext.
        /// </summary>
        public void Dispose()
        {
            //If the Context is valid, Dispose of it
            if (this._context != null)
            {
                this._context.Dispose();
            }
        }
        #endregion
    }
}
