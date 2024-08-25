using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace LightFencing.Equipment.Swords
{
    public class Sword : BaseEquipmentPart
    {
        [SerializeField]
        private GameObject blade;

        [SerializeField]
        private GameObject handle;

        [SerializeField]
        private float restorationTimeInSeconds;

        [SerializeField]
        private InputActionReference activateAction;

        private IBladeVisuals _visuals;

        private float _maxBatteryLevel;
        private float _currentBatteryLevel;

        private bool _bladeActive;
        private bool _bladeDischarged;

        [UsedImplicitly]
        [Inject]
        private void Construct(IBladeVisuals visuals)
        {
            _visuals = visuals;
        }

        private void OnEnable()
        {
            if (!activateAction)
                return;

            activateAction.action.performed += OnActivated;
            activateAction.action.canceled += OnDeactivated;
        }

        private void OnDisable()
        {
            if (!activateAction)
                return;

            activateAction.action.performed -= OnActivated;
            activateAction.action.canceled -= OnDeactivated;
        }

        public void TryTurnBladeOn()
        {
            if (_bladeDischarged)
                return;
            _bladeActive = true;
            _visuals.TurnBladeOn();
        }

        public void TurnBladeOff()
        {
            if (_bladeDischarged)
                return;
            _bladeActive = false;
            _visuals.TurnBladeOff();
        }

        private void DischargeBlade()
        {
            _bladeDischarged = true;
            _visuals.DischargeBlade();
        }

        private async UniTask RestoreBlade(float restorationTime)
        {
            await UniTask.Delay((int)(restorationTime * 1000));
            _bladeDischarged = false;
            TurnBladeOff();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Only active blade reacts to triggers
            if (!_bladeActive) 
                return;

            DischargeBlade();
        }

        private async void OnTriggerExit(Collider other)
        {
            await RestoreBlade(restorationTimeInSeconds);
        }

        private void HandleCollisionWithShield()
        {

        }

        private void HandleCollisionWithArmor()
        {

        }

        private void OnActivated(InputAction.CallbackContext obj)
        {
            TryTurnBladeOn();
        }

        private void OnDeactivated(InputAction.CallbackContext obj)
        {
            TurnBladeOff();
        }
    }
}