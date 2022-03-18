using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEM_Net.Data
{
    interface IRepository
    {
        /// <summary>
        /// A repository interface that provides the basic CRUD operations for every entity type.
        /// </summary>
        /// <typeparam name="T">The entity type for this repository.</typeparam>
        public interface IGenericRepository<T> where T : class
        {
            /// <summary>
            /// Get a database entity by its primary key id.
            /// </summary>
            /// <param name="id">The id of the database entity.</param>
            /// <returns></returns>
            public T GetById(int id);

            /// <summary>
            /// Get all entities of this type.
            /// </summary>
            /// <returns></returns>
            public IEnumerable<T> GetAll();

            /// <summary>
            /// Add an entity to the database.
            /// </summary>
            /// <param name="entity">The entity to be added.</param>
            public void Add(T entity);

            /// <summary>
            /// Add several entities to the database.
            /// </summary>
            /// <param name="entities">The entities to be added.</param>
            public void AddRange(IEnumerable<T> entities);

            /// <summary>
            /// Remove an entity from the database.
            /// </summary>
            /// <param name="entity">The entity to be removed.</param>
            public void Remove(T entity);

            /// <summary>
            /// Remove several entities to be removed.
            /// </summary>
            /// <param name="entities">The entities to be removed.</param>
            public void RemoveRange(IEnumerable<T> entities);
        }
    }
}
