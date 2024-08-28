using JetBrains.Annotations;
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

        public override void AttachToPlayer(Transform bodyPartTransform)
        {
            helmetTracker.StartTracking(bodyPartTransform);
            bodyPartTracker.StartTracking(bodyPartTransform);
        }

        public void HandleBladeHit()
        {
            Debug.Log("Armor hit!");
            _visuals.BladeHit();
        }
    }
}
