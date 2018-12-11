using System;

namespace Data.Mapping
{
    [Flags]
    public enum MapMode
    {
        Keys = 1,
        Values = 2,
        NotNull = 4,
        Both = Keys | Values
    }
}