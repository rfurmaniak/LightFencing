using System;

namespace LightFencing
{
    [Flags]
    public enum Axis : short
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4,
    }
}
