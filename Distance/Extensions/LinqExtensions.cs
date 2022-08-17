using System.Collections.Generic;

namespace System.Linq
{
	public static class LinqExtensions
	{
		public static IEnumerable<TSource> DistinctByHashCode<TSource>(this IEnumerable<TSource> source)
			=> source.DistinctByCriteria(x => x.GetHashCode());

		public static IEnumerable<TSource> DistinctByCriteria<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
			=> source
			.GroupBy(keySelector)
			.Select(x => x.First());

		public static IEnumerable<TResult> SelectManyNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
			=> source
			.Where(x => selector(x) != null)
			.SelectMany(selector);
	}
}
