using LightFencing.Equipment.Shields;
using UnityEngine;

namespace LightFencing
{
    public class LightshieldVisuals : MonoBehaviour, IShieldVisuals
    {
        private static readonly int OutlineColorKeyword = Shader.PropertyToID("_EdgesEmission");
        private static readonly int CausticsColorKeyword = Shader.PropertyToID("_CausticsColor");
        private static readonly int ArmorBaseColorKeyword = Shader.PropertyToID("_BaseColor");

        private static readonly int GeneratorEmissionKeyword = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float causticsEmissionIntensity;

        [SerializeField]
        private float outlineEmissionIntensity;

        [SerializeField]
        private MeshRenderer armorRenderer;

        [SerializeField]
        private MeshRenderer generatorRenderer;

        [SerializeField]
        private ParticleSystem startupEffect;

        [SerializeField]
        private ParticleSystem turnoffEffect;

        [SerializeField]
        private ParticleSystem bladeHitEffect;

        private Color _shieldColor;
        public Color ShieldColor
        {
            get => _shieldColor;
            set
            {
                _shieldColor = value;
                armorRenderer.material.SetColor(OutlineColorKeyword, _shieldColor * outlineEmissionIntensity);
                armorRenderer.material.SetColor(CausticsColorKeyword, _shieldColor * causticsEmissionIntensity);
                armorRenderer.material.SetColor(ArmorBaseColorKeyword, _shieldColor);
                generatorRenderer.material.SetColor(GeneratorEmissionKeyword, _shieldColor * outlineEmissionIntensity);
                var mainModule = turnoffEffect.main;
                //It is impossible to do it with turnoffEffect.main.startColor = _shieldColor because it's a struct
                mainModule.startColor = _shieldColor;
                mainModule = startupEffect.main;
                mainModule.startColor = _shieldColor;
            }
        }

        public void BladeHit(Vector3 hitPosition)
        {
            bladeHitEffect.transform.position = hitPosition;
            bladeHitEffect.Play();
        }

        public void LightDown()
        {
            turnoffEffect.Play();
            animator.SetBool("ShieldEnabled", false);
        }

        public void LightUp()
        {
            animator.SetBool("ShieldEnabled", true);
            startupEffect.Play();
        }
    }
}
