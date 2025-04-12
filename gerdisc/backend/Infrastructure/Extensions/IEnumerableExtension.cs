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
        /// Returns the elements that have been added and removed between two enumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <param name="initialEnumerable">The initial list.</param>
        /// <param name="updatedEnumerable">The updated list.</param>
        /// <returns>A tuple containing the elements that have been added and removed, respectively.</returns>
        public static Tuple<IEnumerable<T>, IEnumerable<T>> IEnumerableDifference<T>(this IEnumerable<T> initialEnumerable, IEnumerable<T> updatedEnumerable)
        {
            return (updatedEnumerable.Except(initialEnumerable), initialEnumerable.Except(updatedEnumerable)).ToTuple();
        }
    }
}
