using System;

namespace Data.Mapping.Attributes
{
    /// <summary>
    /// Marks property as read only, 
    /// it would be changed only on first mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : Attribute
    {
        public ReadOnlyAttribute()
        {
            
        }
    }
}