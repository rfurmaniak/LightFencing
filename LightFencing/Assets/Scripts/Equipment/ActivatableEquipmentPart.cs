using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LightFencing.Equipment
{
    public abstract class ActivatableEquipmentPart : BaseEquipmentPart
    {
        [SerializeField]
        protected InputActionReference activateAction;

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

        protected abstract void OnActivated(InputAction.CallbackContext obj);

        protected abstract void OnDeactivated(InputAction.CallbackContext obj);
    }
}
