using UnityEngine;

namespace LightFencing.Equipment.Swords
{
    public class BasicBladeVisuals : MonoBehaviour, IBladeVisuals
    {
        [SerializeField]
        private MeshRenderer bladeRenderer;

        public void DischargeBlade()
        {
            bladeRenderer.material.color = Color.red;
        }

        public void TurnBladeOff()
        {
            bladeRenderer.material.color = Color.grey;
        }

        public void TurnBladeOn()
        {
            bladeRenderer.material.color = Color.yellow;
        }
    }
}