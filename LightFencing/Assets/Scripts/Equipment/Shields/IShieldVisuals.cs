using UnityEngine;

namespace LightFencing.Equipment.Shields
{
    public interface IShieldVisuals : IBaseEquipmentVisuals
    {
        void LightUp();
        void LightDown();
        void BladeHit(Vector3 hitLocation);
    }
}