using System;

namespace LightFencing.Utils
{
    [Flags]
    public enum BodyPart
    {
        None = 0,
        RightHand = 1,
        LeftHand = 2,
        Head = 4,
        Body = 8,
    }
}
