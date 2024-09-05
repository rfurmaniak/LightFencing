using JetBrains.Annotations;
using LightFencing.Players;
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

        public override void Setup(Player player)
        {
            base.Setup(player);
            _visuals.ShieldColor = player.Color;
        }

        public void HandleBladeHit(Vector3 hitLocation)
        {
            Debug.Log("Shield hit!");
            _visuals.BladeHit(hitLocation);
        }

        public override void Activate()
        {
            armorCollider.enabled = true;
            _visuals.LightUp();
        }

        public override void Deactivate()
        {
            armorCollider.enabled = false;
            _visuals.LightDown();
        }
    }
}