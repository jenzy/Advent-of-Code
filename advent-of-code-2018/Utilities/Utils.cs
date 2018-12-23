﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Utilities
{
    public static class Utils
    {
        public static IEnumerable<T> RepeatForever<T>(this T item)
        {
            for(;;)
                yield return item;

            // ReSharper disable once IteratorNeverReturns
        }

        public static IEnumerable<T> RepeatForever<T>(this IEnumerable<T> seq) => seq.RepeatForever<IEnumerable<T>>().SelectMany(x => x);

        public static IEnumerable<(T1 a, T2 b)> Zip<T1, T2>(this IEnumerable<T1> items1, IEnumerable<T2> items2) => items1.Zip(items2, (a, b) => (a, b));

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        public static (TKey min, TKey max) MinMax<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            comparer = comparer ?? Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                    throw new InvalidOperationException("Sequence contains no elements");

                var max = selector(sourceIterator.Current);
                var min = selector(sourceIterator.Current);
                while (sourceIterator.MoveNext())
                {
                    var candidateProjected = selector(sourceIterator.Current);
                    if (comparer.Compare(candidateProjected, max) > 0)
                        max = candidateProjected;
                    if (comparer.Compare(candidateProjected, min) < 0)
                        min = candidateProjected;
                }
                return (min, max);
            }
        }

        public static char ToUpper(this in char c) => char.ToUpper(c);

        public static bool IsUpper(this in char c) => char.IsUpper(c);

        public static bool TryMatch(this Regex regex, string input, out Match match)
        {
            match = regex.Match(input);
            return match.Success;
        }

        public static string Get(this Match match, string name) => match.Groups[name].Value;

        public static bool HasAtLeast<T>(this IEnumerable<T> enumerable, int amount)
        {
            return enumerable.Take(amount).Count() == amount;
        }

        public static IEnumerable<long> Range(long start, long count, long step = 1)
        {
            var end = start + count;
            for (var i = start; i < end; i += step)
                yield return i;
        }
    }
}
