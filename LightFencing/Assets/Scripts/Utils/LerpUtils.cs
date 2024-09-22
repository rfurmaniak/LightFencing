using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LightFencing.Utils
{
    public static class LerpUtils
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

        public static async UniTask LerpTo(Transform targetTransform, Vector3 targetPosition, Quaternion targetRotation, float lerpTime)
        {
            Vector3 startPosition = targetTransform.position;
            Quaternion startRotation = targetTransform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < lerpTime)
            {
                if (!targetTransform)
                    return;
                elapsedTime += Time.deltaTime;
                targetTransform.SetPositionAndRotation(
                    Vector3.Lerp(startPosition, targetPosition, elapsedTime / lerpTime), 
                    Quaternion.Lerp(startRotation, targetRotation, elapsedTime / lerpTime));
                await UniTask.Yield();
            }

            targetTransform.SetPositionAndRotation(targetPosition, targetRotation);
        }
    }
}
