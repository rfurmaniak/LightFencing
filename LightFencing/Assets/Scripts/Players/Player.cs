using JetBrains.Annotations;
using LightFencing.Core.Interactions;
using LightFencing.Equipment.Armors;
using LightFencing.Equipment.Shields;
using LightFencing.Equipment.Swords;
using UnityEngine;
using Zenject;

namespace LightFencing.Players
{
    public class Player : MonoBehaviour
    {
        public static Player LocalPlayer { get; set; }

        private IDeviceTransformProvider _deviceTransformProvider;

        [SerializeField]
        private Sword sword;

        [SerializeField]
        private Shield shield;

        [SerializeField]
        private Armor armor;

        [field: SerializeField]
        public Color Color { get; private set; }
        public string Id { get; private set; }

        [UsedImplicitly]
        [Inject]
        private void Construct(string id, bool isLocalPlayer, IDeviceTransformProvider deviceTransformProvider)
        {
            Debug.Log("Constructing player");
            _deviceTransformProvider = deviceTransformProvider;

            if (isLocalPlayer)
                LocalPlayer = this;
            Initialize(id);
        }

        public void Initialize(string id)
        {
            Id = id;
            SetupEquipment();
        }

        private void SetupEquipment()
        {
            //I could use base class and iterate over collection, but it's just those three pieces and I don't plan on adding more
            sword.Setup(this);
            shield.Setup(this);
            armor.Setup(this);
            sword.AttachToPlayer(_deviceTransformProvider.GetControllerTransform(Handedness.Right));
            shield.AttachToPlayer(_deviceTransformProvider.GetControllerTransform(Handedness.Left));
            armor.AttachToPlayer(_deviceTransformProvider.GetHeadTransform());
        }

        public class PlayerFactory : PlaceholderFactory<string, bool, Player>
        { 
        }

    }
}
