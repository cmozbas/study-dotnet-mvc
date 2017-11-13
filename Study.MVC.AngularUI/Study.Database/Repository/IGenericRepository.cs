using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Study.Database.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQueryable();

        IQueryable<TEntity> GetQueryableIncluded(params Expression<Func<TEntity, object>>[] includeProperties);
        
        Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> querySharper);

        void AddCollection(ICollection<TEntity> entitiesToAdd);

        void DeleteCollection(ICollection<TEntity> entitiesToAdd);
    }


    public interface IUnitOfWork<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        Task SaveAsync();
    }

    /// <summary>
    /// new(): It has to have a default constructor. It can't be an int or double.  
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IUnitOfWork<TEntity>
        where TEntity : class
    {
        private DbContext context { get; set; }
        protected DbSet<TEntity> dbEntity { get; set; }

        protected const int DefaultPagedSize = 50;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbEntity = context.Set<TEntity>();
        }

        #region Generic interface

        public IQueryable<TEntity> GetQueryable()
        {
            IQueryable<TEntity> query = dbEntity;
            return query;
        }

        /// <summary>
        /// Gets the included queryable.
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>IQueryable entity.</returns>
        /// <exception cref="System.ArgumentNullException">includeProperties</exception>
        /// <example>
        /// var submission = this.GetIncludedQueryable(new Expression/Func/entity_name, object\\[]{
        ///        t => t.entity_sub_selection
        ///    })
        /// </example>
        public IQueryable<TEntity> GetQueryableIncluded(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (includeProperties == null)
                throw new ArgumentNullException("includeProperties");

            IQueryable<TEntity> query = dbEntity;

            // Include properties
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// blogs.msdn.microsoft.com/mrtechnocal/2014/03/16/asynchronous-repositories/
        /// </summary>
        /// <param name="querySharper"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> querySharper)
        {
            var query = querySharper(context.Set<TEntity>());
            return await query.ToListAsync();
        }
        
        /// <summary>
        /// Deletes the specified entities to delete.
        /// </summary>
        /// <param name="entitiesToAdd">The entity to delete.</param>
        public virtual void AddCollection(ICollection<TEntity> entitiesToAdd)
        {
            context.Set<TEntity>().AddRange(entitiesToAdd);
        }

        /// <summary>
        /// Deletes the specified entities to delete.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public virtual void DeleteCollection(ICollection<TEntity> entitiesToDelete)
        {
            context.Set<TEntity>().RemoveRange(entitiesToDelete);
        }

        #endregion

        #region Unit of work

        public virtual void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public virtual void Edit(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        #endregion
    }
}
