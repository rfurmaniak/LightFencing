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
        private IArmorAudio _audio;

        public Transform HelmetTransform => helmetTracker.transform;
        public Transform BodyTransform => bodyPartTracker.transform;

        protected override IBaseEquipmentVisuals Visuals => _visuals;

        [UsedImplicitly]
        [Inject]
        private void Construct(IArmorVisuals visuals, IArmorAudio audio)
        {
            _visuals = visuals;
            _audio = audio;
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
            _audio.BladeHit(hitPosition);
            ArmorHit?.Invoke();
        }
    }
}
