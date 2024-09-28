using UnityEngine;

namespace LightFencing.Equipment.Armors
{
    public class ScifiArmorAudio : MonoBehaviour, IArmorAudio
    {
        [SerializeField]
        private AudioSource hitEffect;

        public void BladeHit(Vector3 hitPosition)
        {
            transform.position = hitPosition;
            hitEffect.Play();
        }
    }
}
