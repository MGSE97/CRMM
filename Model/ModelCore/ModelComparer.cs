using System;
using System.Collections.Generic;

namespace ModelCore
{
    public class ModelComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> _hash;
        private readonly Func<T, T, bool> _comparer;


        public ModelComparer(Func<T, object> hash,Func<T, T, bool> comparer)
        {
            _hash = hash;
            _comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hash(obj).GetHashCode();
        }
    }
}