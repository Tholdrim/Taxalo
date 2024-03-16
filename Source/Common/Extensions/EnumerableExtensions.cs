namespace Taxalo
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(TSource Item, int Index)> WithIndex<TSource>(this IEnumerable<TSource> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}
