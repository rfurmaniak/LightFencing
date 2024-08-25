using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LightFencing.Equipment.Shields
{
    public class BasicShieldVisuals : MonoBehaviour, IShieldVisuals
    {
        [SerializeField]
        private MeshRenderer shieldArmorRenderer;

        private Color _startColor;

        private void Awake()
        {
            _startColor = shieldArmorRenderer.material.color;
            LightDown();
        }

        public async void BladeHit()
        {
            await LerpColor(Color.yellow, 0.25f);
            await LerpColor(_startColor, 0.25f);
        }

        public async void LightDown()
        {
            await LerpColor(Color.blue, 0.25f);
            await LerpColor(new Color(0,0,1,0), 0.25f);
        }

        public async void LightUp()
        {
            await LerpColor(Color.green, 0.25f);
            await LerpColor(_startColor, 0.25f);
        }

        public async UniTask LerpColor(Color destinationColor, float lerpTime)
        {
            Color startColor = shieldArmorRenderer.material.color;
            float elapsedTime = 0f;

            while (elapsedTime < lerpTime)
            {
                elapsedTime += Time.deltaTime;
                shieldArmorRenderer.material.color = Color.Lerp(startColor, destinationColor, elapsedTime / lerpTime);
                await UniTask.Yield();
            }

            shieldArmorRenderer.material.color = destinationColor;
        }
    }
}
