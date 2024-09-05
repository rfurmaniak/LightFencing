using UnityEngine;
using UnityEngine.InputSystem;

namespace LightFencing.Core.Configs
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        [field: SerializeField]
        public InputActionReference ActivateSwordAction { get; private set; }

        [field: SerializeField]
        public InputActionReference ActivateShieldAction { get; private set; }
    }
}
