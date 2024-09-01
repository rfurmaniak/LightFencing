using UnityEngine;

namespace LightFencing.Equipment.Armors
{

    public interface IArmorVisuals
    {
        Color ArmorColor { get; set; }
        void BladeHit();
    }
}
