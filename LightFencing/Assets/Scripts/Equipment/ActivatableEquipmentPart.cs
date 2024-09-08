using LightFencing.Players;

namespace LightFencing.Equipment
{
    public abstract class ActivatableEquipmentPart : BaseEquipmentPart
    {
        protected Battery Battery;

        public abstract void Activate();

        public abstract void Deactivate();

        public override void Setup(Player player)
        {
            base.Setup(player);
            Battery = player.Battery;
            Battery.BatteryDrained += OnBatteryDrained;
        }

        protected virtual void OnDestroy()
        {
            if (Battery)
                Battery.BatteryDrained -= OnBatteryDrained;
        }

        private void OnBatteryDrained()
        {
            Deactivate();
        }
    }
}
