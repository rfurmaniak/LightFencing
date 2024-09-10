using UnityEngine;

namespace LightFencing.Core.Configs
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Configs/MainConfig")]
    public class MainConfig : ScriptableObject
    {
        [field: SerializeField]
        public bool AllowSelfHit { get; private set; }

        [field: SerializeField]
        public float SwordDischargeTime { get; private set; }

        [field: SerializeField]
        public float ArmorHitCooldown { get; private set; }

        [field: SerializeField]
        public int BatteryDrainSpeed { get; private set; }

        [field: SerializeField]
        public int BatteryRechargeSpeed { get; private set; }

        [field: SerializeField]
        public int MaxBatteryLevel { get; private set; }

        [field: SerializeField]
        public int BatteryUpdateMillisecondsInterval { get; private set; }

        [field: SerializeField]
        public int BatteryDrainedMillisecondsTime { get; private set; }

        [field: SerializeField]
        public int MaxHealth { get; private set; }
    }
}
