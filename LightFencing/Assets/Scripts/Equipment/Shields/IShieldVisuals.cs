using UnityEngine;

namespace LightFencing.Equipment.Shields
{
    public interface IShieldVisuals
    {
        Color ShieldColor { get; set; }
        void LightUp();
        void LightDown();
        void BladeHit(Vector3 hitLocation);
    }
}