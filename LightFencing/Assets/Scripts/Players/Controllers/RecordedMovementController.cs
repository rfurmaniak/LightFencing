using Cysharp.Threading.Tasks;
using LightFencing.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LightFencing.Players.Controllers
{
    public class RecordedMovementController : MonoBehaviour, IEnemyController
    {
        [SerializeField]
        private Transform referenceTransform;

        [SerializeField]
        private Transform headTransform;

        [SerializeField]
        private Transform swordHandTransform;

        [SerializeField]
        private Transform shieldHandTransform;

        public Transform HeadTransform => headTransform;

        public Transform SwordHandTransform => swordHandTransform;

        public Transform ShieldHandTransform => shieldHandTransform;

        private bool _isPlaying;
        private int _currentFrameIndex;
        private List<MovementFrame> _frames;

        public void Clear()
        {
            _isPlaying = false;
            _frames.Clear();
        }

        public async void Initialize(Player player)
        {
            await LoadRecording(Path.Combine(Application.persistentDataPath, "recording.json"));
            _isPlaying = true;
        }

        private async UniTask LoadRecording(string path)
        {
            var data = await File.ReadAllTextAsync(path);
            _frames = JsonConvert.DeserializeObject<List<MovementFrame>>(data);
        }

        private void Update()
        {
            if (!_isPlaying)
                return;

            if (_currentFrameIndex >= _frames.Count - 1)
                _currentFrameIndex = 0;

            var currentFrame = _frames[_currentFrameIndex];

            headTransform.position = referenceTransform.TransformPoint(currentFrame.HeadPosition);
            swordHandTransform.position = referenceTransform.TransformPoint(currentFrame.RightHandPosition);
            shieldHandTransform.position = referenceTransform.TransformPoint(currentFrame.LeftHandPosition);

            headTransform.rotation = referenceTransform.rotation * Quaternion.Euler(currentFrame.HeadRotation);
            swordHandTransform.rotation = referenceTransform.rotation * Quaternion.Euler(currentFrame.RightHandRotation);
            shieldHandTransform.rotation = referenceTransform.rotation * Quaternion.Euler(currentFrame.LeftHandRotation);

            _currentFrameIndex++;
        }
    }
}
