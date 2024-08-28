using UnityEngine;

namespace LightFencing.Equipment.Armors
{
    public class ArmorReference : MonoBehaviour
    {
        [field: SerializeField]
        public Armor Armor { get; private set; }
    }
}
