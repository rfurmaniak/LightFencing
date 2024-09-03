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

        [SerializeField]
        private ParticleSystem bladeHitEffect;

        private Color _armorColor;
        public Color ArmorColor
        {
            get => _armorColor;
            set
            {
                _armorColor = value;
                helmetRenderer.material.SetColor(EmissionKeyword, _armorColor * emissionIntensity);
            }
        }

        public void BladeHit(Vector3 hitPosition)
        {
            var lookVector = transform.position - hitPosition;
            var rotation = Quaternion.LookRotation(lookVector, Vector3.up);
            bladeHitEffect.transform.SetPositionAndRotation(hitPosition, rotation);
            bladeHitEffect.Play();
        }

        public void SetHelmetVisibility(bool helmetVisibility)
        {
            helmetRenderer.enabled = helmetVisibility;
        }
    }
}
