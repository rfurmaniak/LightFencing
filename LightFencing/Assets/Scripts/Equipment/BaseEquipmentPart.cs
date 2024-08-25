using Dev.Agred.Tools.AttachAttributes;
using LightFencing.Utils;
using UnityEngine;

namespace LightFencing.Equipment
{
    [RequireComponent(typeof(BodyPartTracker))]
    public class BaseEquipmentPart : MonoBehaviour
    {
        [SerializeField]
        [GetComponent]
        protected BodyPartTracker handTracker;

        public string PlayerId { get; protected set; }

        public void Setup(string playerId)
        {
            PlayerId = playerId;
        }

        public void AttachToHand(Transform controllerTransform)
        {
            handTracker.StartTracking(controllerTransform);
        }
    }
}