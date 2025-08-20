using Microsoft.Xrm.Sdk;
using System.Threading;
using System.Threading.Tasks;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Provides extension methods for Entity class.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Safely retrieves a typed attribute value from an entity
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute value</typeparam>
        /// <param name="entity">The entity containing the attribute</param>
        /// <param name="attributeName">The logical name of the attribute to retrieve</param>
        /// <param name="defaultValue">The default value to return if the attribute doesn't exist or is of wrong type</param>
        /// <returns>The attribute value as the specified type, or the default value</returns>
        public static T TryGet<T>(this Entity entity, string attributeName, T defaultValue = default)
        {
            if (entity != null && entity.Contains(attributeName) && entity[attributeName] is T value)
                return value;
            return defaultValue;
        }

        /// <summary>
        /// Asynchronously retrieves a typed attribute value from an entity
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute value</typeparam>
        /// <param name="entity">The entity containing the attribute</param>
        /// <param name="attributeName">The logical name of the attribute to retrieve</param>
        /// <param name="defaultValue">The default value to return if the attribute doesn't exist or is of wrong type</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning the attribute value as the specified type, or the default value</returns>
        public static Task<T> TryGetAsync<T>(this Entity entity, string attributeName, T defaultValue = default, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entity.TryGet(attributeName, defaultValue), cancellationToken);
        }
    }
}