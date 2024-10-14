using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArticlesManagement.Infrastructure.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }


        public static IEnumerable<TSource> SkipIf<TSource>(this IEnumerable<TSource> source, bool condition, int number)
        {
            if (condition)
                return source.Skip(number);
            else
                return source;
        }

        public static IEnumerable<TSource> SkipIf<TSource>(this IQueryable<TSource> source, bool condition, int number)
        {
            if (condition)
                return source.Skip(number);
            else
                return source;
        }

        public static IEnumerable<TSource> SkipIf<TSource>(this IEnumerable<TSource> source, int? number)
        {
            if (number != null)
                return source.Skip((int)number);
            else
                return source;
        }

        public static IEnumerable<TSource> SkipIf<TSource>(this IQueryable<TSource> source, int? number)
        {
            if (number != null)
                return source.Skip((int)number);
            else
                return source;
        }

        public static IEnumerable<TSource> TakeIf<TSource>(this IEnumerable<TSource> source, bool condition, int number)
        {
            if (condition)
                return source.Take(number);
            else
                return source;
        }

        public static IEnumerable<TSource> TakeIf<TSource>(this IQueryable<TSource> source, bool condition, int number)
        {
            if (condition)
                return source.Take(number);
            else
                return source;
        }

    }
}
