using UnityEngine;

namespace LightFencing.Equipment.Swords
{
    public interface IBladeVisuals
    {
        Color SwordColor { get; set; }
        void TurnBladeOn();
        void TurnBladeOff();
        void DischargeBlade();
    }
}