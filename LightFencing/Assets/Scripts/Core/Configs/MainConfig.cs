using UnityEngine;

namespace LightFencing
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/MainConfig")]
    public class MainConfig : ScriptableObject
    {
        [field: SerializeField]
        public bool AllowSelfHit { get; private set; }

        [field: SerializeField]
        public float SwordDischargeTime { get; private set; }
    }
}
