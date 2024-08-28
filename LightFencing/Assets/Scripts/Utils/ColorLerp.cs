using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LightFencing.Utils
{
    public static class ColorLerp
    {
        public static async UniTask LerpColor(Material material, Color destinationColor, float lerpTime)
        {
            Color startColor = material.color;
            float elapsedTime = 0f;

            while (elapsedTime < lerpTime)
            {
                elapsedTime += Time.deltaTime;
                material.color = Color.Lerp(startColor, destinationColor, elapsedTime / lerpTime);
                await UniTask.Yield();
            }

            material.color = destinationColor;
        }
    }
}
