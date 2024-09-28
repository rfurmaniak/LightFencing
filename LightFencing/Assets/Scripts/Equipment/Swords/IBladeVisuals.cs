namespace LightFencing.Equipment.Swords
{
    public interface IBladeVisuals : IBaseEquipmentVisuals
    {
        void TurnBladeOn();
        void TurnBladeOff();
        void DischargeBlade();
    }
}