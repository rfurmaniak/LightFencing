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
        private float causticsEmissionIntensity;

        [SerializeField]
        private float outlineEmissionIntensity;

        [SerializeField]
        private MeshRenderer armorRenderer;

        [SerializeField]
        private MeshRenderer generatorRenderer;

        private Color _shieldColor;
        public Color ShieldColor
        {
            get => _shieldColor;
            set
            {
                Debug.Log("Assigning color");
                _shieldColor = value;
                armorRenderer.material.SetColor(OutlineColorKeyword, _shieldColor * outlineEmissionIntensity);
                armorRenderer.material.SetColor(CausticsColorKeyword, _shieldColor * causticsEmissionIntensity);
                armorRenderer.material.SetColor(ArmorBaseColorKeyword, _shieldColor);
                generatorRenderer.material.SetColor(GeneratorEmissionKeyword, _shieldColor * outlineEmissionIntensity);
            }
        }

        public void BladeHit()
        {
        }

        public void LightDown()
        {
            armorRenderer.enabled = false;
        }

        public void LightUp()
        {
            armorRenderer.enabled = true;
        }
    }
}
