using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace LightFencing.Equipment.Swords
{
    public class Sword : BaseEquipmentPart
    {
        [SerializeField]
        private IBladeVisuals visuals;

        [SerializeField]
        private GameObject blade;

        [SerializeField]
        private GameObject handle;

        [SerializeField]
        private float restorationTimeInSeconds;

        [SerializeField]
        private InputActionReference activateAction;

        [SerializeField]
        private InputActionReference deactivateAction;

        private float _maxBatteryLevel;
        private float _currentBatteryLevel;

        private bool _bladeDischarged;

        private void Awake()
        {
            activateAction.action.performed += OnActivated;
            deactivateAction.action.performed += OnDeactivated;
        }

        private void OnDeactivated(InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnActivated(InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            InputActionManager oko;
        }

        public void TurnBladeOn()
        {
            visuals.TurnBladeOn();
        }

        public void TurnBladeOff()
        {
            visuals.TurnBladeOff();
        }

        private void DischargeBlade()
        {
            _bladeDischarged = true;
            visuals.DischargeBlade();
            TurnBladeOff();
        }

        private async UniTask RestoreBlade(float restorationTime)
        {
            await UniTask.Delay((int)(restorationTime * 1000));
            _bladeDischarged = false;
        }

        private void OnTriggerEnter(Collider other)
        {
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
    }
}