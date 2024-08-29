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

        public virtual void Setup(Player player)
        {
            PlayerId = player.Id;
        }

        public virtual void AttachToPlayer(Transform bodyPartTransform)
        {
            bodyPartTracker.StartTracking(bodyPartTransform);
        }
    }
}