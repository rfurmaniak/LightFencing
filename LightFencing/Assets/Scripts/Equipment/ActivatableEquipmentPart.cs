using LightFencing.Players;
using System;

namespace LightFencing.Equipment
{
    public abstract class ActivatableEquipmentPart : BaseEquipmentPart
    {
        public event Action Activated;
        public event Action Deactivated;
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

        protected void InvokeActivated()
        {
            Activated?.Invoke();
        }

        protected void InvokeDeactivated()
        {
            Deactivated?.Invoke();
        }

        private void OnBatteryDrained()
        {
            Deactivate();
        }
    }
}
