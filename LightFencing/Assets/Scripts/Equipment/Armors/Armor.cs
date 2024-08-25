using LightFencing.Utils;
using UnityEngine;

namespace LightFencing.Equipment.Armors
{
    public class Armor : MonoBehaviour
    {
        [SerializeField]
        private BodyPartTracker helmetTracker;

        [SerializeField]
        private BodyPartTracker armorTracker;

        public void TrackUser(Transform userHead)
        {
            helmetTracker.StartTracking(userHead);
            armorTracker.StartTracking(userHead);
        }
    }
}
