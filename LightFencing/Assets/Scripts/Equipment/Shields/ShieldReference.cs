using UnityEngine;

namespace LightFencing.Equipment.Shields
{
    /// <summary>
    /// Simple component to cache reference to shield. Used to avoid ugly GetComponentInParent 
    /// </summary>
    public class ShieldReference : MonoBehaviour
    {
        [field: SerializeField]
        public Shield Shield {  get; private set; }
    }
}
