using Dev.Agred.Tools.AttachAttributes;
using LightFencing.Utils;
using UnityEngine;

namespace LightFencing.Equipment
{
    public abstract class BaseEquipmentPart : MonoBehaviour
    {
        [SerializeField]
        protected BodyPartTracker bodyPartTracker;

        public string PlayerId { get; protected set; }

        public void Setup(string playerId)
        {
            PlayerId = playerId;
        }

        public virtual void AttachToPlayer(Transform bodyPartTransform)
        {
            bodyPartTracker.StartTracking(bodyPartTransform);
        }
    }
}