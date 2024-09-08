using JetBrains.Annotations;
using LightFencing.Players;
using LightFencing.Utils;
using System;
using UnityEngine;
using Zenject;

namespace LightFencing.Equipment.Armors
{
    public class Armor : BaseEquipmentPart
    {
        public event Action ArmorHit;

        [SerializeField]
        private BodyPartTracker helmetTracker;

        private IArmorVisuals _visuals;

        protected override IBaseEquipmentVisuals Visuals => _visuals;

        [UsedImplicitly]
        [Inject]
        private void Construct(IArmorVisuals visuals)
        {
            _visuals = visuals;
        }

        public override void Setup(Player player)
        {
            base.Setup(player);
            _visuals.SetHelmetVisibility(player.Id != Player.LocalPlayer.Id);
        }

        public override void AttachToPlayer(Transform bodyPartTransform)
        {
            helmetTracker.StartTracking(bodyPartTransform);
            bodyPartTracker.StartTracking(bodyPartTransform);
        }

        public void HandleBladeHit(Vector3 hitPosition)
        {
            Debug.Log("Armor hit!");
            _visuals.BladeHit(hitPosition);
            ArmorHit?.Invoke();
        }
    }
}
