using System;
using System.Collections.Generic;

namespace Services
{
    public static class Extensions
    {
        public static IList<T> ForEach<T>(this IList<T> list, Action<T> action)
        {
            if(action != null)
                foreach (var item in list)
                {
                    action.Invoke(item);
                }

            return list;
        }

        public static T Try<T>(this T item, Action<T> action, Action<T, Exception> exception = null)
        {
            try
            {
                action?.Invoke(item);
            }
            catch (Exception ex)
            {
                exception?.Invoke(item, ex);
            }

            return item;
        }
    }
}