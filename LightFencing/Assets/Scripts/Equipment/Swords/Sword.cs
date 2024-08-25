using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LightFencing.Equipment.Shields;
using LightFencing.Players;
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

        private IBladeVisuals _visuals;

        private MainConfig _mainConfig;

        private float _maxBatteryLevel;
        private float _currentBatteryLevel;

        private bool _bladeActive;
        private bool _bladeDischarged;

        private float DischargeTime => _mainConfig.SwordDischargeTime;
        private bool AllowSelfHit => _mainConfig.AllowSelfHit;

        [UsedImplicitly]
        [Inject]
        private void Construct(MainConfig mainConfig, IBladeVisuals visuals)
        {
            _mainConfig = mainConfig;
            _visuals = visuals;
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

            var shield = other.GetComponent<ShieldReference>().Shield;
            HandleCollisionEnterWithShield(shield);
        }

        private void OnTriggerExit(Collider other)
        {
            var shield = other.GetComponent<ShieldReference>().Shield;
            HandleCollisionExitWithShield(shield);
        }

        private void HandleCollisionEnterWithShield(Shield shield)
        {
            if (!AllowSelfHit && shield.PlayerId == Player.LocalPlayer.Id)
                return;
            shield.HandleBladeHit();
            DischargeBlade();
        }

        private async void HandleCollisionExitWithShield(Shield shield)
        {
            if (!AllowSelfHit && shield.PlayerId == Player.LocalPlayer.Id)
                return;
            await RestoreBlade(DischargeTime);
        }

        private void HandleCollisionWithArmor()
        {

        }

        protected override void OnActivated(InputAction.CallbackContext obj)
        {
            TryTurnBladeOn();
        }

        protected override void OnDeactivated(InputAction.CallbackContext obj)
        {
            TurnBladeOff();
        }
    }
}