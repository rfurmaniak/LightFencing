using LightFencing.Players;
using LightFencing.Utils;
using UnityEngine;

namespace LightFencing.Equipment
{
    public abstract class BaseEquipmentPart : MonoBehaviour
    {
        [SerializeField]
        protected BodyPartTracker bodyPartTracker;

        public string PlayerId { get; protected set; }

        protected abstract IBaseEquipmentVisuals Visuals { get; }

        private Color _color;
        public virtual Color Color 
        {
            get => _color;
            set
            {
                _color = value;
                Visuals.Color = Color;
            }
        }

        public virtual void Setup(Player player)
        {
            PlayerId = player.Id;
            Visuals.Color = player.Color;
        }

        public virtual void AttachToPlayer(Transform bodyPartTransform)
        {
            bodyPartTracker.StartTracking(bodyPartTransform);
        }
    }
}