using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
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

        [UsedImplicitly]
        [Inject]
        private void Construct(IShieldVisuals visuals)
        {
            _visuals = visuals;
        }

        public void HandleBladeHit()
        {
            Debug.Log("Shield hit!");
            _visuals.BladeHit();
        }

        protected override void OnActivated(InputAction.CallbackContext obj)
        {
            armorCollider.enabled = true;
            _visuals.LightUp();
        }

        protected override void OnDeactivated(InputAction.CallbackContext obj)
        {
            armorCollider.enabled = false;
            _visuals.LightDown();
        }
    }
}