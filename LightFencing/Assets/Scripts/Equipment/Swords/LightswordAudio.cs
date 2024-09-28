using UnityEngine;

namespace LightFencing.Equipment.Swords
{
    public class LightswordAudio : MonoBehaviour, IBladeAudio
    {
        [SerializeField]
        private AudioSource dischargeSound;

        [SerializeField]
        private AudioSource bladeOffEffect;

        [SerializeField]
        private AudioSource bladeOnEffect;

        [SerializeField]
        private AudioSource bladeIdleSound;

        public void DischargeBlade()
        {
            dischargeSound.Play();
        }

        public void TurnBladeOff()
        {
            bladeOffEffect.Play();
            bladeIdleSound.Stop();
        }

        public void TurnBladeOn()
        {
            bladeOnEffect.Play();
            bladeIdleSound.Play();
        }
    }
}
