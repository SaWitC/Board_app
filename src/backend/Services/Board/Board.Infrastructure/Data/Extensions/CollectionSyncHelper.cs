namespace Board.Infrastructure.Data.Extensions;

public static class CollectionSyncHelper
{
    public static void Synchronize<TSource, TDestination, TKey>(
        this ICollection<TDestination> destinationCollection,
        IEnumerable<TSource> sourceCollection,
        Func<TDestination, TKey> destinationKeySelector,
        Func<TSource, TKey> sourceKeySelector,
        Action<TDestination, TSource> updateAction,
        Func<TSource, TDestination> createAction) where TDestination : class
    {
        Synchronize(destinationCollection, sourceCollection, destinationKeySelector, sourceKeySelector, updateAction, createAction, EqualityComparer<TKey>.Default);
    }

    public static void Synchronize<TSource, TDestination, TKey>(
        this ICollection<TDestination> destinationCollection,
        IEnumerable<TSource> sourceCollection,
        Func<TDestination, TKey> destinationKeySelector,
        Func<TSource, TKey> sourceKeySelector,
        Action<TDestination, TSource> updateAction,
        Func<TSource, TDestination> createAction,
        IEqualityComparer<TKey> keyComparer) where TDestination : class
    {
        // Create dictionaries for efficient lookups (O(1) average time complexity)
        var destinationMap = destinationCollection.ToDictionary(destinationKeySelector, keyComparer);
        var sourceMap = sourceCollection.ToDictionary(sourceKeySelector, keyComparer);

        // Precompute source keys set with provided comparer
        var sourceKeys = new HashSet<TKey>(sourceMap.Keys, keyComparer);

        // --- 1. Remove items that are in the destination but not in the source ---
        var itemsToRemove = destinationCollection
            .Where(item => !sourceKeys.Contains(destinationKeySelector(item)))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            destinationCollection.Remove(item);
        }

        // --- 2. Update existing items and add new ones ---
        foreach (var sourceItem in sourceCollection)
        {
            var sourceKey = sourceKeySelector(sourceItem);

            // If an item with the same key exists in the destination, update it
            if (destinationMap.TryGetValue(sourceKey, out var destinationItem))
            {
                updateAction(destinationItem, sourceItem);
            }
            // Otherwise, create a new item and add it to the destination
            else
            {
                var newItem = createAction(sourceItem);
                destinationCollection.Add(newItem);
            }
        }
    }
}
