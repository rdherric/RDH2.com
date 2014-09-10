using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
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
        /// Default constructor for the Repository object that creates
        /// the ModelContext directly with the default properties.
        /// </summary>
        public Repository()
        {
            //Create a new ModelContext
            this._context = new ModelContext();
        }

        
        /// <summary>
        /// Constructor that takes a connection string to connect
        /// to the data store.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database</param>
        public Repository(String connectionString)
        {
            //Create a new ModelContext
            this._context = new ModelContext(connectionString);
        }
        #endregion


        #region IRepository Implementation
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
            T rtn = this._context.Set<T>().Attach(entity);

            //Save the changes to the database
            this._context.SaveChanges();

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
                //Attach the entity to the data store
                rtn = this._context.Set<T>().Add(entity);

                //Save the changes to the database
                this._context.SaveChanges();
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
            //Declare a variable to return
            DbEntityEntry rtn = this._context.Entry(entity);

            //Attach the new object
            this._context.Set<T>().Attach(entity);

            //Set the entry as modified and save it
            rtn.State = EntityState.Modified;
            this._context.SaveChanges();

            //Return the result
            return rtn as T;
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
        /// Dispose cleans out the DbContext when the object is
        /// Garbage Collected.
        /// </summary>
        public void Dispose()
        {
            //Clean up the Context
            this._context.Dispose();
        }
        #endregion
    }
}
