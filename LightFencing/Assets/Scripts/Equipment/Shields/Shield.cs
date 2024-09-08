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
        private bool _shieldActivated;

        protected override IBaseEquipmentVisuals Visuals => _visuals;

        [UsedImplicitly]
        [Inject]
        private void Construct(IShieldVisuals visuals)
        {
            _visuals = visuals;
        }

        public void HandleBladeHit(Vector3 hitLocation)
        {
            Debug.Log("Shield hit!");
            _visuals.BladeHit(hitLocation);
        }

        public override void Activate()
        {
            if (_shieldActivated)
                return;
            _shieldActivated = true;
            armorCollider.enabled = true;
            Battery.StartUsingBattery();
            _visuals.LightUp();
        }

        public override void Deactivate()
        {
            if (!_shieldActivated)
                return;
            _shieldActivated = false;
            armorCollider.enabled = false;
            Battery.StopUsingBattery();
            _visuals.LightDown();
        }
    }
}