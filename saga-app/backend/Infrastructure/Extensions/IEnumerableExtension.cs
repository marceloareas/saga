using System;
using System.Collections.Generic;
using System.Linq;

namespace saga.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for enumerable.
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Returns (added, removed) between two enumerables.
        /// </summary>
        public static Tuple<IEnumerable<T>, IEnumerable<T>> IEnumerableDifference<T>(
            this IEnumerable<T> initialEnumerable,
            IEnumerable<T> updatedEnumerable)
        {
            var added   = updatedEnumerable.Except(initialEnumerable);
            var removed = initialEnumerable.Except(updatedEnumerable);
            return new Tuple<IEnumerable<T>, IEnumerable<T>>(added, removed);
        }
    }
}
