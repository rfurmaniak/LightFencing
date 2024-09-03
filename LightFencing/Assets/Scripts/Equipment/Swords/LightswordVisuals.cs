using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LightFencing.Equipment.Swords
{
    public class LightswordVisuals : MonoBehaviour, IBladeVisuals
    {
        private static readonly int BaseColorKeyword = Shader.PropertyToID("_BaseColor");
        private static readonly int EmissionKeyword = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float emissionIntensity;

        [SerializeField]
        private MeshRenderer bladeRenderer;

        [SerializeField]
        private MeshRenderer handleRenderer;

        [SerializeField]
        private ParticleSystem idleEffect;

        [SerializeField]
        private ParticleSystem startupEffect;

        [SerializeField]
        private ParticleSystem turnoffEffect;

        [SerializeField]
        private ParticleSystem dischargeEffect;

        [SerializeField]
        private ParticleSystem trailEffect;

        private Color _swordColor;
        public Color SwordColor
        {
            get => _swordColor;
            set
            {
                _swordColor = value;
                var emissiveColor = _swordColor;
                emissiveColor *= emissionIntensity;
                bladeRenderer.material.SetColor(BaseColorKeyword, emissiveColor);
                bladeRenderer.material.SetColor(EmissionKeyword, emissiveColor);
                handleRenderer.material.SetColor(EmissionKeyword, emissiveColor);
                var mainModule = turnoffEffect.main;
                //It is impossible to do it with turnoffEffect.main.startColor = _swordColor because it's a struct
                mainModule.startColor = _swordColor;
                mainModule = startupEffect.main;
                mainModule.startColor = _swordColor;
                mainModule = idleEffect.main;
                mainModule.startColor = _swordColor;
                mainModule = dischargeEffect.main;
                mainModule.startColor = _swordColor;
                mainModule = trailEffect.main;
                mainModule.startColor = _swordColor;
            }
        }

        public async void DischargeBlade()
        {
            animator.SetTrigger("Discharge");
            dischargeEffect.Play();
            await UniTask.DelayFrame(1);
            animator.SetBool("BladeEnabled", false);
        }

        public void TurnBladeOn()
        {
            animator.SetBool("BladeEnabled", true);
            startupEffect.Play();
        }

        public void TurnBladeOff()
        {
            animator.SetBool("BladeEnabled", false);
            turnoffEffect.Play();
        }
    }
}
