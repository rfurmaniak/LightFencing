using UnityEngine;

namespace LightFencing.Equipment.Armors
{
    public class ScifiArmorVisuals : MonoBehaviour, IArmorVisuals
    {
        private static readonly int EmissionKeyword = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private float emissionIntensity;

        [SerializeField]
        private MeshRenderer helmetRenderer;

        private Color _armorColor;
        public Color ArmorColor
        {
            get => _armorColor;
            set
            {
                Debug.Log("Assigning color");
                _armorColor = value;
                helmetRenderer.material.SetColor(EmissionKeyword, _armorColor * emissionIntensity);
            }
        }

        public void BladeHit()
        {
        }
    }
}
