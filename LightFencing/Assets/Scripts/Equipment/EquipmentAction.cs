using System;

namespace LightFencing.Equipment
{
    [Flags]
    public enum EquipmentAction
    {
        None = 0,
        SwordActivated = 1,
        SwordDeactivated = 2,
        ShieldActivated = 4,
        ShieldDeactivated = 8,
    }
}
