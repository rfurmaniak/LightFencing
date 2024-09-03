using JetBrains.Annotations;
using LightFencing.Players;
using LightFencing.Utils;
using UnityEngine;
using Zenject;

namespace LightFencing.Equipment.Armors
{
    public class Armor : BaseEquipmentPart
    {
        [SerializeField]
        private BodyPartTracker helmetTracker;

        private IArmorVisuals _visuals;

        [UsedImplicitly]
        [Inject]
        private void Construct(IArmorVisuals visuals)
        {
            _visuals = visuals;
        }

        public override void Setup(Player player)
        {
            base.Setup(player);
            _visuals.ArmorColor = player.Color;
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
        }
    }
}
