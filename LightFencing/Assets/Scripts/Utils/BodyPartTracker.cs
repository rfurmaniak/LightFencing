using UnityEngine;

namespace LightFencing.Utils
{
    public class BodyPartTracker : MonoBehaviour
    {
        [SerializeField]
        private bool trackRotation = true;

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
            if (trackRotation)
            {
                transform.rotation = Quaternion.Euler(_transformToTrack.rotation.eulerAngles + rotationOffset);
            }
        }
    }
}