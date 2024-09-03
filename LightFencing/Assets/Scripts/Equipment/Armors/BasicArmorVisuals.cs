using Cysharp.Threading.Tasks;
using UnityEngine;
using static LightFencing.Utils.ColorLerp;

namespace LightFencing.Equipment.Armors
{
    public class BasicArmorVisuals : MonoBehaviour, IArmorVisuals
    {
        [SerializeField]
        private MeshRenderer armorRenderer;

        [SerializeField]
        private MeshRenderer helmetRenderer;

        private Color _startColor;
        private Material _armorMaterial;
        private Material _helmetMaterial;

        public Color ArmorColor { get;  set; }

        private void Awake()
        {
            _armorMaterial = armorRenderer.material;
            _helmetMaterial = helmetRenderer.material;
            _startColor = _armorMaterial.color;
        }

        public async void BladeHit(Vector3 hitPosition)
        {
            //Yes, this is awful, but it's just for quick testing
            LerpColor(_armorMaterial, Color.red, 0.25f).Forget();
            await LerpColor(_helmetMaterial, Color.red, 0.25f);
            LerpColor(_armorMaterial, _startColor, 0.25f).Forget();
            await LerpColor(_helmetMaterial, _startColor, 0.25f);
        }

        public void SetHelmetVisibility(bool helmetVisibility)
        {
            helmetRenderer.enabled = helmetVisibility;
        }
    }
}
