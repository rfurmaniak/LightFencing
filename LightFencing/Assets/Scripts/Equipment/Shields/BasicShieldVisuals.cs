using UnityEngine;
using static LightFencing.Utils.ColorLerp;

namespace LightFencing.Equipment.Shields
{
    public class BasicShieldVisuals : MonoBehaviour, IShieldVisuals
    {
        [SerializeField]
        private MeshRenderer shieldArmorRenderer;

        private Color _startColor;
        private Material _shieldMaterial;

        public Color Color { get; set; }

        private void Awake()
        {
            _shieldMaterial = shieldArmorRenderer.material;
            _startColor = _shieldMaterial.color;
            LightDown();
        }

        public async void BladeHit(Vector3 hitPosition)
        {
            await LerpColor(_shieldMaterial, Color.yellow, 0.25f);
            await LerpColor(_shieldMaterial, _startColor, 0.25f);
        }

        public async void LightDown()
        {
            await LerpColor(_shieldMaterial, Color.blue, 0.25f);
            await LerpColor(_shieldMaterial, new Color(0, 0, 1, 0), 0.25f);
        }

        public async void LightUp()
        {
            await LerpColor(_shieldMaterial, Color.green, 0.25f);
            await LerpColor(_shieldMaterial, _startColor, 0.25f);
        }
    }
}
