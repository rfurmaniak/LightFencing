using JetBrains.Annotations;
using LightFencing.Core.Interactions;
using LightFencing.Equipment.Armors;
using LightFencing.Equipment.Shields;
using LightFencing.Equipment.Swords;
using LightFencing.Players;
using System;
using UnityEngine;
using Zenject;

namespace LightFencing.Core.Managers
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField]
        private Player localPlayer;

        private IDeviceTransformProvider _deviceTransformProvider;
        private Sword _sword;
        private Shield _shield;
        private Armor _armor;

        [UsedImplicitly]
        [Inject]
        private void Construct(IDeviceTransformProvider deviceTransformProvider, Sword sword, Shield shield, Armor armor)
        {
            Debug.Log("Constructing game manager");
            _deviceTransformProvider = deviceTransformProvider;
            _sword = sword;
            _shield = shield;
            _armor = armor;
        }

        private void Start()
        {
            InitializeLocalPlayer();
            SetupEquipment();
        }

        private void InitializeLocalPlayer()
        {
            localPlayer.Id = Guid.NewGuid().ToString();
            Player.LocalPlayer = localPlayer;
        }

        private void SetupEquipment()
        {
            //I could use base class and iterate over collection, but it's just those three pieces and I don't plan on adding more
            _sword.Setup(localPlayer);
            _shield.Setup(localPlayer);
            _armor.Setup(localPlayer);
            _sword.AttachToPlayer(_deviceTransformProvider.GetControllerTransform(Handedness.Right));
            _shield.AttachToPlayer(_deviceTransformProvider.GetControllerTransform(Handedness.Left));
            _armor.AttachToPlayer(_deviceTransformProvider.GetHeadTransform());
        }
    }
}