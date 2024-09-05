using LightFencing.Core.Configs;
using LightFencing.Core.Interactions;
using LightFencing.Players;
using LightFencing.Players.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace LightFencing
{
    public class LocalPlayerController : IPlayerController
    {
        private readonly InputActionReference _activateSwordAction;
        private readonly InputActionReference _activateShieldAction;

        private IDeviceTransformProvider _deviceTransformProvider;

        public Transform HeadTransform => _deviceTransformProvider.GetHeadTransform();

        public Transform SwordHandTransform => _deviceTransformProvider.GetControllerTransform(Handedness.Right);

        public Transform ShieldHandTransform => _deviceTransformProvider.GetControllerTransform(Handedness.Left);

        private Player _player;

        [Inject]
        public LocalPlayerController(InputConfig inputConfig, IDeviceTransformProvider deviceTransformProvider)
        {
            _deviceTransformProvider = deviceTransformProvider;
            _activateSwordAction = inputConfig.ActivateSwordAction;
            _activateShieldAction = inputConfig.ActivateShieldAction;
        }

        public void Initialize(Player player)
        {
            _player = player;
            _activateShieldAction.action.performed += OnShieldActivated;
            _activateShieldAction.action.canceled += OnShieldDeactivated;

            _activateSwordAction.action.performed += OnSwordActivated;
            _activateSwordAction.action.canceled += OnSwordDeactivated;
        }

        public void Clear()
        {
            _activateShieldAction.action.performed -= OnShieldActivated;
            _activateShieldAction.action.canceled -= OnShieldDeactivated;

            _activateSwordAction.action.performed -= OnSwordActivated;
            _activateSwordAction.action.canceled -= OnSwordDeactivated;
        }

        private void OnShieldActivated(InputAction.CallbackContext obj)
        {
            _player.Shield.Activate();
        }

        private void OnShieldDeactivated(InputAction.CallbackContext obj)
        {
            _player.Shield.Deactivate();
        }

        private void OnSwordActivated(InputAction.CallbackContext obj)
        {
            _player.Sword.Activate();
        }
        private void OnSwordDeactivated(InputAction.CallbackContext obj)
        {
            _player.Sword.Deactivate();
        }
    }
}
