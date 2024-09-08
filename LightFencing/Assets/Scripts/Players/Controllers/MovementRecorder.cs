using Cysharp.Threading.Tasks;
using LightFencing.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LightFencing.Players.Controllers
{
    public class MovementRecorder : MonoBehaviour
    {
        [SerializeField]
        private Transform referenceTransform;

        private Transform headTransform;
        private Transform bodyTransform;
        private Transform rightHandTransform;
        private Transform leftHandTransform;

        [SerializeField]
        private InputActionReference recordButton;

        private readonly List<MovementFrame> _frames = new();
        private readonly Stopwatch _stopwatch = new();
        private bool _isRecording;

        private void Awake()
        {
            recordButton.action.performed += SwitchRecording;
        }

        private void OnDestroy()
        {
            recordButton.action.performed -= SwitchRecording;
        }

        private void SwitchRecording(InputAction.CallbackContext obj)
        {
            if (!_isRecording)
            {
                StartRecording();
                return;
            }

            StopRecording();
        }

        public void StartRecording()
        {
            var localPlayer = Player.LocalPlayer;
            headTransform = localPlayer.Armor.HelmetTransform;
            bodyTransform = localPlayer.Armor.BodyTransform;
            rightHandTransform = localPlayer.Sword.SwordTransform;
            leftHandTransform = localPlayer.Shield.ShieldTransform;

            _frames.Clear();
            _stopwatch.Restart();
            _isRecording = true;
        }

        public void StopRecording()
        {
            _isRecording = false;
            _stopwatch.Stop();
            SaveRecording(Path.Join(Application.persistentDataPath, "recording.json")).Forget();
        }

        private void Update()
        {
            //Yeah, if I just record every frame and then 
            //just replay it in every frame the framerate will affect the movement.
            //However, this is purely for testing and should not be used in actual gameplay.
            //I could use the timestamp to sample the position and interpolate between frames using real time,
            //but I don't have time for that right now.

            if (_isRecording)
                _frames.Add(new MovementFrame(_stopwatch.ElapsedMilliseconds, referenceTransform, headTransform, bodyTransform, leftHandTransform, rightHandTransform));
        }

        private async UniTask SaveRecording(string path)
        {
            var serializedData = JsonConvert.SerializeObject(_frames);
            await File.WriteAllTextAsync(path, serializedData);
        }
    }
}
