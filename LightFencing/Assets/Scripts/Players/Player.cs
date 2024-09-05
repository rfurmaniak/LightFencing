using JetBrains.Annotations;
using LightFencing.Core.Interactions;
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

        private IDeviceTransformProvider _deviceTransformProvider;
        private AbstractPlayerController _controller;

        [field: SerializeField]
        public Sword Sword { get; private set; }

        [field: SerializeField]
        public Shield Shield { get; private set; }

        [field: SerializeField]
        public Armor Armor { get; private set; }

        [field: SerializeField]
        public Color Color { get; private set; }

        public string Id { get; private set; }

        [UsedImplicitly]
        [Inject]
        private void Construct(string id, bool isLocalPlayer, AbstractPlayerController controller, IDeviceTransformProvider deviceTransformProvider)
        {
            Debug.Log("Constructing player");

            _controller = controller;
            _deviceTransformProvider = deviceTransformProvider;

            if (isLocalPlayer)
                LocalPlayer = this;
            Initialize(id);
        }

        private void OnDestroy()
        {
            if (_controller != null)
                _controller.Clear();
        }

        public void Initialize(string id)
        {
            Id = id;
            SetupEquipment();
            _controller.Initialize(this);
        }

        private void SetupEquipment()
        {
            //I could use base class and iterate over collection, but it's just those three pieces and I don't plan on adding more
            Sword.Setup(this);
            Shield.Setup(this);
            Armor.Setup(this);
            Sword.AttachToPlayer(_controller.SwordHandTransform);
            Shield.AttachToPlayer(_controller.ShieldHandTransform);
            Armor.AttachToPlayer(_controller.HeadTransform);
        }

        public class PlayerFactory : PlaceholderFactory<string, bool, AbstractPlayerController, Player>
        {
        }

    }
}
