using UnityEngine;

namespace LightFencing.Utils
{
    public class BodyPartTracker : MonoBehaviour
    {
        [SerializeField]
        private Axis lockRotation;

        [SerializeField]
        private Vector3 positionOffset;

        [SerializeField]
        private Vector3 rotationOffset;

        private Transform _transformToTrack;
        private bool _isTracking;

        public void StartTracking(Transform controllerTransform)
        {
            _transformToTrack = controllerTransform;
            StartTracking();
        }

        public void StartTracking()
        {
            _isTracking = true;
        }

        public void StopTracking()
        {
            _isTracking = false;
        }

        private void Update()
        {
            if (!_isTracking || !_transformToTrack)
            {
                return;
            }

            transform.position = _transformToTrack.position + positionOffset;

            var rotationToSet = _transformToTrack.rotation.eulerAngles + rotationOffset;

            rotationToSet.x = lockRotation.HasFlag(Axis.X) ? 0 : rotationToSet.x;
            rotationToSet.y = lockRotation.HasFlag(Axis.Y) ? 0 : rotationToSet.y;
            rotationToSet.z = lockRotation.HasFlag(Axis.Z) ? 0 : rotationToSet.z;
            
            transform.rotation = Quaternion.Euler(rotationToSet);
        }
    }
}