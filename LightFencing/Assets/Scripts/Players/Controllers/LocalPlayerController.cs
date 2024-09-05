using LightFencing.Core.Configs;
using LightFencing.Core.Interactions;
using LightFencing.Players;
using LightFencing.Players.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace LightFencing
{
    public class LocalPlayerController : AbstractPlayerController
    {
        private readonly InputActionReference _activateSwordAction;
        private readonly InputActionReference _activateShieldAction;

        private IDeviceTransformProvider _deviceTransformProvider;

        public override Transform HeadTransform => _deviceTransformProvider.GetHeadTransform();

        public override Transform SwordHandTransform => _deviceTransformProvider.GetControllerTransform(Handedness.Right);

        public override Transform ShieldHandTransform => _deviceTransformProvider.GetControllerTransform(Handedness.Left);

        [Inject]
        public LocalPlayerController(InputConfig inputConfig, IDeviceTransformProvider deviceTransformProvider)
        {
            _deviceTransformProvider = deviceTransformProvider;
            _activateSwordAction = inputConfig.ActivateSwordAction;
            _activateShieldAction = inputConfig.ActivateShieldAction;
        }

        public override void Initialize(Player player)
        {
            base.Initialize(player);
            _activateShieldAction.action.performed += OnShieldActivated;
            _activateShieldAction.action.canceled += OnShieldDeactivated;

            _activateSwordAction.action.performed += OnSwordActivated;
            _activateSwordAction.action.canceled += OnSwordDeactivated;
        }

        public override void Clear()
        {
            _activateShieldAction.action.performed -= OnShieldActivated;
            _activateShieldAction.action.canceled -= OnShieldDeactivated;

            _activateSwordAction.action.performed -= OnSwordActivated;
            _activateSwordAction.action.canceled -= OnSwordDeactivated;
        }

        private void OnShieldActivated(InputAction.CallbackContext obj)
        {
            Player.Shield.Activate();
        }

        private void OnShieldDeactivated(InputAction.CallbackContext obj)
        {
            Player.Shield.Deactivate();
        }

        private void OnSwordActivated(InputAction.CallbackContext obj)
        {
            Player.Sword.Activate();
        }
        private void OnSwordDeactivated(InputAction.CallbackContext obj)
        {
            Player.Sword.Deactivate();
        }
    }
}
