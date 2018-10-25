using System;

namespace Data.Mapping
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class KeyAttribute : Attribute
    {
    }
}