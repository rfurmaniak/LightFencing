using UnityEngine;

namespace LightFencing.Equipment.Swords
{
    public class BasicBladeVisuals : MonoBehaviour, IBladeVisuals
    {
        [SerializeField]
        private MeshRenderer bladeRenderer;

        private Color _swordColor = Color.gray;
        public Color Color
        {
            get => _swordColor;
            set
            {
                _swordColor = value;
                bladeRenderer.material.color = _swordColor;
            }
        }

        public void DischargeBlade()
        {
            bladeRenderer.material.color = Color.red;
        }

        public void TurnBladeOff()
        {
            bladeRenderer.material.color = _swordColor;
        }

        public void TurnBladeOn()
        {
            bladeRenderer.material.color = Color.yellow;
        }
    }
}