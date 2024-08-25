using Dev.Agred.Tools.AttachAttributes;
using LightFencing.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LightFencing.Equipment
{
    [RequireComponent(typeof(BodyPartTracker))]
    public abstract class BaseEquipmentPart : MonoBehaviour
    {
        [SerializeField]
        [GetComponent]
        protected BodyPartTracker handTracker;

        [SerializeField]
        protected InputActionReference activateAction;

        public string PlayerId { get; protected set; }

        public void Setup(string playerId)
        {
            PlayerId = playerId;
        }

        protected virtual void OnEnable()
        {
            if (!activateAction)
                return;

            activateAction.action.performed += OnActivated;
            activateAction.action.canceled += OnDeactivated;
        }

        protected virtual void OnDisable()
        {
            if (!activateAction)
                return;

            activateAction.action.performed -= OnActivated;
            activateAction.action.canceled -= OnDeactivated;
        }

        public void AttachToHand(Transform controllerTransform)
        {
            handTracker.StartTracking(controllerTransform);
        }

        protected abstract void OnActivated(InputAction.CallbackContext obj);

        protected abstract void OnDeactivated(InputAction.CallbackContext obj);
    }
}