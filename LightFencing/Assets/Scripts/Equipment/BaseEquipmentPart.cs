using Dev.Agred.Tools.AttachAttributes;
using LightFencing.Tests;
using UnityEngine;

namespace LightFencing.Equipment
{
    [RequireComponent(typeof(BodyPartTracker))]
    public class BaseEquipmentPart : MonoBehaviour
    {
        [SerializeField]
        [GetComponent]
        protected BodyPartTracker handTracker;

        public void AttachToHand(Transform controllerTransform)
        {
            handTracker.StartTracking(controllerTransform);
        }
    }
}