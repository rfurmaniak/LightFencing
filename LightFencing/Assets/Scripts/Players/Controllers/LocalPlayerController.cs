using LightFencing.Players;
using LightFencing.Players.Controllers;
using UnityEngine.InputSystem;

namespace LightFencing
{
    public class LocalPlayerController : AbstractPlayerController
    {
        private readonly InputActionReference _activateSwordAction;
        private readonly InputActionReference _activateShieldAction;

        public LocalPlayerController(InputActionReference activateSwordAction, InputActionReference activateShieldAction)
        {
            _activateSwordAction = activateSwordAction;
            _activateShieldAction = activateShieldAction;
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
