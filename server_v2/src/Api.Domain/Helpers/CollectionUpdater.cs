using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Helpers
{
    public static class CollectionUpdater
    {
        public static void Sync<T, TKey>(
            ICollection<T> current,
            IEnumerable<T> updated,
            Func<T, TKey> keySelector)
        {
            var updatedList = updated.ToList();

            // remover
            var toRemove = current
                .Where(c => !updatedList.Any(u => 
                    EqualityComparer<TKey>.Default.Equals(keySelector(c), keySelector(u))))
                .ToList();

            foreach (var item in toRemove)
                current.Remove(item);

            // adicionar
            var toAdd = updatedList
                .Where(u => !current.Any(c => 
                    EqualityComparer<TKey>.Default.Equals(keySelector(c), keySelector(u))))
                .ToList();

            foreach (var item in toAdd)
                current.Add(item);
        }
    }
}