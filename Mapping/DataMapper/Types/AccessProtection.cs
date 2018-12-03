using System;

namespace Data.Mapping
{
    [Flags]
    public enum AccessProtection
    {
        Private = 1,
        Protected = 2,
        Public = 4,
        Any = Public|Protected|Private,

        Widows = Public,
        Horo = Protected,
        Sasuke = Private
    }
}