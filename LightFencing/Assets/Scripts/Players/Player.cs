using JetBrains.Annotations;
using LightFencing.Core.Configs;
using LightFencing.Equipment;
using LightFencing.Equipment.Armors;
using LightFencing.Equipment.Shields;
using LightFencing.Equipment.Swords;
using LightFencing.Players.Controllers;
using UnityEngine;
using Zenject;

namespace LightFencing.Players
{
    public class Player : MonoBehaviour
    {
        public static Player LocalPlayer { get; set; }

        private IPlayerController _controller;
        private Color _color;
        private BaseEquipmentPart[] _equipmentParts;

        [field: SerializeField]
        public Sword Sword { get; private set; }

        [field: SerializeField]
        public Shield Shield { get; private set; }

        [field: SerializeField]
        public Armor Armor { get; private set; }

        [field: SerializeField]
        public Battery Battery { get; private set; }

        public string Id { get; private set; }

        public int CurrentHealth { get; private set; }

        public int MaxHealth { get; private set; }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                foreach (var part in _equipmentParts)
                {
                    part.Color = _color;
                }
            }
        }

        [UsedImplicitly]
        [Inject]
        private void Construct(MainConfig mainConfig, string id, bool isLocalPlayer, IPlayerController controller)
        {
            Debug.Log("Constructing player");
            _equipmentParts = new BaseEquipmentPart[] { Sword, Shield, Armor };
            _controller = controller;

            CurrentHealth = MaxHealth = mainConfig.MaxHealth;

            if (isLocalPlayer)
                LocalPlayer = this;
            Initialize(id);
        }

        private void OnDestroy()
        {
            _controller?.Clear();
            if (Armor)
                Armor.ArmorHit -= OnArmorHit;
        }

        public void Initialize(string id)
        {
            Id = id;
            SetupEquipment();
            _controller.Initialize(this);
        }

        private void SetupEquipment()
        {
            foreach (var part in _equipmentParts)
            {
                part.Setup(this);
            }
            Sword.AttachToPlayer(_controller.SwordHandTransform);
            Shield.AttachToPlayer(_controller.ShieldHandTransform);
            Armor.AttachToPlayer(_controller.HeadTransform);

            Armor.ArmorHit += OnArmorHit;
        }

        private void OnArmorHit()
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - 1, 0, MaxHealth);
        }

        public class PlayerFactory : PlaceholderFactory<string, bool, IPlayerController, Player>
        {
        }

    }
}
