using UnityEngine;

namespace LightFencing.Equipment.Swords
{
    public class LightswordVisuals : MonoBehaviour, IBladeVisuals
    {
        private static readonly int BaseColorKeyword = Shader.PropertyToID("_BaseColor");
        private static readonly int EmissionKeyword = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private float emissionIntensity;

        [SerializeField]
        private MeshRenderer bladeRenderer;

        [SerializeField]
        private MeshRenderer handleRenderer;

        private Color _swordColor;
        public Color SwordColor
        {
            get => _swordColor;
            set
            {
                Debug.Log("Assigning color");
                _swordColor = value;
                var emissiveColor = _swordColor;
                emissiveColor *= emissionIntensity;
                bladeRenderer.material.SetColor(BaseColorKeyword, emissiveColor);
                bladeRenderer.material.SetColor(EmissionKeyword, emissiveColor);
                handleRenderer.material.SetColor(EmissionKeyword, emissiveColor);
            }
        }

        public void DischargeBlade()
        {

        }

        public void TurnBladeOn()
        {

        }

        public void TurnBladeOff()
        {

        }
    }
}
