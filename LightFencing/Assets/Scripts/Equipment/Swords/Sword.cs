using UnityEngine;

namespace LightFencing.Equipment.Swords
{
    public class Sword : BaseEquipmentPart
    {
        [SerializeField]
        private GameObject blade;

        [SerializeField]
        private GameObject handle;

        private void OnTriggerEnter(Collider other)
        {
            blade.GetComponent<MeshRenderer>().enabled = false;
        }

        private void OnTriggerExit(Collider other)
        {
            blade.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}