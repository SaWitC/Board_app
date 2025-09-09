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
        // Create dictionaries for efficient lookups (O(1) average time complexity)
        var destinationMap = destinationCollection.ToDictionary(destinationKeySelector);
        var sourceMap = sourceCollection.ToDictionary(sourceKeySelector);

        // --- 1. Remove items that are in the destination but not in the source ---
        var keysToRemove = destinationMap.Keys.Except(sourceMap.Keys);
        var itemsToRemove = destinationCollection
            .Where(item => keysToRemove.Contains(destinationKeySelector(item)))
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
