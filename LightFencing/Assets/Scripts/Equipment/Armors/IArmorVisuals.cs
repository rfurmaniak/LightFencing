using UnityEngine;

namespace LightFencing.Equipment.Armors
{

    public interface IArmorVisuals : IBaseEquipmentVisuals
    {
        void BladeHit(Vector3 hitPosition);
        void SetHelmetVisibility(bool helmetVisibility);
    }
}
