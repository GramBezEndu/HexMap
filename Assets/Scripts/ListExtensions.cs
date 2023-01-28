using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static IEnumerable<List<T>> Chunk<T>(this List<T> source, int count)
    {
        for (int i = 0; i < source.Count; i += count)
        {
            yield return source.GetRange(i, Math.Min(count, source.Count - i));
        }
    }
}
