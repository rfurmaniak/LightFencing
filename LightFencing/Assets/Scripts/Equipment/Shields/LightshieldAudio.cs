using UnityEngine;

namespace LightFencing.Equipment.Shields
{
    public class LightshieldAudio : MonoBehaviour, IShieldAudio
    {
        [SerializeField]
        private AudioSource bladeHitSound;

        [SerializeField]
        private AudioSource lightUpSound;

        [SerializeField]
        private AudioSource lightDownSound;

        public void BladeHit(Vector3 hitLocation)
        {
            bladeHitSound.Play();
        }

        public void LightDown()
        {
            lightDownSound.Play();
        }

        public void LightUp()
        {
            lightUpSound.Play();
        }
    }
}
