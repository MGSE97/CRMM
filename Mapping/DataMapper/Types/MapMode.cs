using System;

namespace Data.Mapping
{
    [Flags]
    public enum MapMode
    {
        Keys = 1,
        Values = 2,
        Both = Keys | Values
    }
}