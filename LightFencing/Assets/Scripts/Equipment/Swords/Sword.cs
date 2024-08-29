using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LightFencing.Equipment.Armors;
using LightFencing.Equipment.Shields;
using LightFencing.Players;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace LightFencing.Equipment.Swords
{
    public class Sword : ActivatableEquipmentPart
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
        private bool _armorHasBeenHit;

        private float DischargeTime => _mainConfig.SwordDischargeTime;
        private bool AllowSelfHit => _mainConfig.AllowSelfHit;

        [UsedImplicitly]
        [Inject]
        private void Construct(MainConfig mainConfig, IBladeVisuals visuals)
        {
            _mainConfig = mainConfig;
            _visuals = visuals;
        }

        public override void Setup(Player player)
        {
            base.Setup(player);
            _visuals.SwordColor = player.Color;
        }

        protected override void OnActivated(InputAction.CallbackContext obj)
        {
            TryTurnBladeOn();
        }

        protected override void OnDeactivated(InputAction.CallbackContext obj)
        {
            TurnBladeOff();
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
            if (!_bladeActive || _bladeDischarged) 
                return;

            Debug.Log("Collision with active blade detected");
            if (other.CompareTag(Tags.T_Shield))
            {
                HandleCollisionEnterWithShield(other);
                return;
            }

            if (other.CompareTag(Tags.T_Armor))
            {
                HandleCollisionEnterWithArmor(other);
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.T_Shield))
            {
                HandleCollisionExitWithShield(other);
                return;
            }

            if (other.CompareTag(Tags.T_Armor))
            {
                HandleCollisionExitWithArmor(other);
                return;
            }
        }

        private void HandleCollisionEnterWithShield(Collider other)
        {
            var shield = other.GetComponent<ShieldReference>().Shield;
            if (CheckForLocalEquipment(shield))
                return;
            shield.HandleBladeHit();
            DischargeBlade();
        }

        private async void HandleCollisionExitWithShield(Collider other)
        {
            var shield = other.GetComponent<ShieldReference>().Shield;
            if (CheckForLocalEquipment(shield))
                return;
            await RestoreBlade(DischargeTime);
        }

        private void HandleCollisionEnterWithArmor(Collider other)
        {
            Debug.Log("Collision with armor detected");
            if (_armorHasBeenHit) 
                return;
            Debug.Log("Collision with armor detected2");
            var armor = other.GetComponent<ArmorReference>().Armor;
            if (CheckForLocalEquipment(armor))
                return;
            armor.HandleBladeHit();
            //This will decrease player's health
            _armorHasBeenHit = true;
        }

        private async void HandleCollisionExitWithArmor(Collider other)
        {
            var armor = other.GetComponent<ArmorReference>().Armor;
            if (CheckForLocalEquipment(armor))
                return;
            await ArmorHitCooldown();
        }

        private async UniTask ArmorHitCooldown()
        {
            await UniTask.Delay((int)(_mainConfig.ArmorHitCooldown * 1000));
            _armorHasBeenHit = false;
        }

        private bool CheckForLocalEquipment(BaseEquipmentPart equipmentPart)
        {
            return !AllowSelfHit && equipmentPart.PlayerId == Player.LocalPlayer.Id;
        }
    }
}