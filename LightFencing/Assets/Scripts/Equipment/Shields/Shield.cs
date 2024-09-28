using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace LightFencing.Equipment.Shields
{
    public class Shield : ActivatableEquipmentPart
    {
        [SerializeField]
        private GameObject shieldGenerator;

        [SerializeField]
        private GameObject shieldArmor;

        [SerializeField]
        private Collider armorCollider;

        private IShieldVisuals _visuals;
        private IShieldAudio _audio;

        private bool _shieldActivated;

        public Transform ShieldTransform => transform;
        protected override IBaseEquipmentVisuals Visuals => _visuals;

        [UsedImplicitly]
        [Inject]
        private void Construct(IShieldVisuals visuals, IShieldAudio audio)
        {
            _visuals = visuals;
            _audio = audio;
        }

        public void HandleBladeHit(Vector3 hitLocation)
        {
            Debug.Log("Shield hit!");
            _visuals.BladeHit(hitLocation);
            _audio.BladeHit(hitLocation);
        }

        public override void Activate()
        {
            if (_shieldActivated && Battery.CurrentBatteryLevel > 0)
                return;
            _shieldActivated = true;
            armorCollider.enabled = true;
            Battery.StartUsingBattery();
            _visuals.LightUp();
            _audio.LightUp();
            InvokeActivated();
        }

        public override void Deactivate()
        {
            if (!_shieldActivated)
                return;
            _shieldActivated = false;
            armorCollider.enabled = false;
            Battery.StopUsingBattery();
            _visuals.LightDown();
            _audio.LightDown();
            InvokeDeactivated();
        }
    }
}