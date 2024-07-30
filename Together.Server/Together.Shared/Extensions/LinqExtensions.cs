namespace Together.Shared.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> Paging<T>(this IQueryable<T> query, int pageIndex, int pageSize)
    {
        return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
    }
    
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> input)
    {
        return input.Select((item, index) => (item, index));
    }
}