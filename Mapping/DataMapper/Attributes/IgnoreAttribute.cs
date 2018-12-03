using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute()
        {
            
        }
    }
}
