using Cysharp.Threading.Tasks;
using LightFencing.Core.Interactions;
using LightFencing.Equipment;
using LightFencing.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

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
        private readonly object _actionLock = new();
        private bool _isRecording;
        private EquipmentAction _currentActions;

        [Inject]
        private void Construct(IDeviceTransformProvider transformProvider)
        {
            headTransform = transformProvider.GetHeadTransform();
            rightHandTransform = transformProvider.GetControllerTransform(Handedness.Right);
            leftHandTransform = transformProvider.GetControllerTransform(Handedness.Left);
        }

        private void Awake()
        {
            recordButton.action.performed += SwitchRecording;
        }

        private void OnDestroy()
        {
            UnregisterPlayerEvents();
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
            bodyTransform = localPlayer.Armor.BodyTransform;

            localPlayer.Sword.Activated += OnSwordActivated;
            localPlayer.Sword.Deactivated += OnSwordDeactivated;
            localPlayer.Shield.Activated += OnShieldActivated;
            localPlayer.Shield.Deactivated += OnShieldDeactivated;

            _frames.Clear();
            _stopwatch.Restart();
            _currentActions = EquipmentAction.None;
            _isRecording = true;
        }

        public void StopRecording()
        {
            UnregisterPlayerEvents();
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

            if (!_isRecording)
                return;

            _frames.Add(new MovementFrame(_stopwatch.ElapsedMilliseconds, referenceTransform, headTransform, bodyTransform, leftHandTransform, rightHandTransform, _currentActions));
            lock (_actionLock)
                _currentActions = EquipmentAction.None;
        }

        private async UniTask SaveRecording(string path)
        {
            var serializedData = JsonConvert.SerializeObject(_frames);
            await File.WriteAllTextAsync(path, serializedData);
        }

        private void OnShieldDeactivated()
        {
            lock (_actionLock)
                _currentActions |= EquipmentAction.ShieldDeactivated;
        }

        private void OnShieldActivated()
        {
            lock (_actionLock)
                _currentActions |= EquipmentAction.ShieldActivated;
        }

        private void OnSwordDeactivated()
        {
            lock (_actionLock)
                _currentActions |= EquipmentAction.SwordDeactivated;
        }

        private void OnSwordActivated()
        {
            lock (_actionLock)
                _currentActions |= EquipmentAction.SwordActivated;
        }

        private void UnregisterPlayerEvents()
        {
            var localPlayer = Player.LocalPlayer;
            if (!localPlayer)
                return;
            localPlayer.Sword.Activated -= OnSwordActivated;
            localPlayer.Sword.Deactivated -= OnSwordDeactivated;
            localPlayer.Shield.Activated -= OnShieldActivated;
            localPlayer.Shield.Deactivated -= OnShieldDeactivated;
        }
    }
}
