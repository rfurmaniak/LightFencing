using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LightFencing.Core.Configs;
using LightFencing.Equipment.Armors;
using LightFencing.Equipment.Shields;
using System;
using UnityEngine;
using Zenject;

namespace LightFencing.Equipment.Swords
{
    public class Sword : ActivatableEquipmentPart
    {
        public event Action SwordDischarged;

        [SerializeField]
        private GameObject blade;

        [SerializeField]
        private GameObject handle;

        private IBladeVisuals _visuals;
        private IBladeAudio _audio;

        private MainConfig _mainConfig;

        private bool _bladeActive;
        private bool _armorHasBeenHit;
        private bool _armorCooldownActivated;

        public bool IsSwordDischarged { get; private set; }
        public Transform SwordTransform => transform;
        protected override IBaseEquipmentVisuals Visuals => _visuals;
        private float DischargeTime => _mainConfig.SwordDischargeTime;
        private bool AllowSelfHit => _mainConfig.AllowSelfHit;

        [UsedImplicitly]
        [Inject]
        private void Construct(MainConfig mainConfig, IBladeVisuals visuals, IBladeAudio audio)
        {
            _mainConfig = mainConfig;
            _visuals = visuals;
            _audio = audio;
        }

        public override void Activate()
        {
            TryTurnBladeOn();
        }

        public override void Deactivate()
        {
            TurnBladeOff();
        }

        public void TryTurnBladeOn()
        {
            if (IsSwordDischarged || _bladeActive && Battery.CurrentBatteryLevel > 0)
                return;
            _bladeActive = true;
            Battery.StartUsingBattery();
            _visuals.TurnBladeOn();
            _audio.TurnBladeOn();
            InvokeActivated();
        }

        public void TurnBladeOff()
        {
            if (IsSwordDischarged || !_bladeActive)
                return;
            DeactivationActions();
            _visuals.TurnBladeOff();
            _audio.TurnBladeOff();
        }

        private void DeactivationActions()
        {
            _bladeActive = false;
            Battery.StopUsingBattery();
            InvokeDeactivated();
        }

        private void DischargeBlade()
        {
            IsSwordDischarged = true;
            SwordDischarged?.Invoke();
            DeactivationActions();
            _visuals.DischargeBlade();
            _audio.DischargeBlade();

            RestoreBlade(DischargeTime).Forget();
        }

        private async UniTask RestoreBlade(float restorationTime)
        {
            await UniTask.Delay((int)(restorationTime * 1000));
            IsSwordDischarged = false;
            TurnBladeOff();
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Only active blade reacts to triggers
            if (!_bladeActive || IsSwordDischarged)
                return;

            var other = collision.collider;

            Debug.Log("Collision with active blade detected");
            if (other.CompareTag(Tags.T_Shield))
            {
                HandleCollisionEnterWithShield(collision);
                return;
            }

            if (other.CompareTag(Tags.T_Armor))
            {
                HandleCollisionEnterWithArmor(collision);
                return;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            var other = collision.collider;

            if (other.CompareTag(Tags.T_Armor))
            {
                HandleCollisionExitWithArmor(other);
                return;
            }
        }

        private void HandleCollisionEnterWithShield(Collision collision)
        {
            var shield = collision.collider.GetComponent<ShieldReference>().Shield;
            if (CheckForLocalEquipment(shield))
                return;
            shield.HandleBladeHit(collision.GetContact(0).point);
            DischargeBlade();
        }

        private async void HandleCollisionExitWithShield(Collider other)
        {
            var shield = other.GetComponent<ShieldReference>().Shield;
            if (CheckForLocalEquipment(shield))
                return;
            await RestoreBlade(DischargeTime);
        }

        private void HandleCollisionEnterWithArmor(Collision collision)
        {
            if (_armorHasBeenHit)
                return;
            var armor = collision.collider.GetComponent<ArmorReference>().Armor;
            if (CheckForLocalEquipment(armor))
                return;
            _armorCooldownActivated = false;
            _armorHasBeenHit = true;
            armor.HandleBladeHit(collision.GetContact(0).point);
        }

        private async void HandleCollisionExitWithArmor(Collider other)
        {
            if (_armorCooldownActivated)
                return;
            var armor = other.GetComponent<ArmorReference>().Armor;
            if (CheckForLocalEquipment(armor))
                return;
            await ArmorHitCooldown();
        }

        private async UniTask ArmorHitCooldown()
        {
            _armorCooldownActivated = true;
            await UniTask.Delay((int)(_mainConfig.ArmorHitCooldown * 1000));
            _armorHasBeenHit = false;
        }

        private bool CheckForLocalEquipment(BaseEquipmentPart equipmentPart)
        {
            return !AllowSelfHit && equipmentPart.PlayerId == PlayerId;
        }
    }
}