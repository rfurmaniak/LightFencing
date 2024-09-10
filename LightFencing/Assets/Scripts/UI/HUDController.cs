using LightFencing.Players;
using LightFencing.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace LightFencing.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField]
        private Image healthBar;

        [SerializeField]
        private Image energyBar;

        private Player _player;

        public void Setup(Player player)
        {
            _player = player;
            if (_player.Id != Player.LocalPlayer.Id)
                gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_player)
                return;

            energyBar.fillAmount = _player.Battery.CurrentBatteryLevel.Remap(0, _player.Battery.MaxBatteryLevel, 0, 1);
            healthBar.fillAmount = _player.CurrentHealth.Remap(0, _player.MaxHealth, 0, 1);
        }
    }
}
