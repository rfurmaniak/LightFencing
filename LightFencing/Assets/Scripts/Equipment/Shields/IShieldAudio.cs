using UnityEngine;

namespace LightFencing.Equipment.Shields
{
    public interface IShieldAudio
    {
        void LightUp();
        void LightDown();
        void BladeHit(Vector3 hitLocation);
    }
}
