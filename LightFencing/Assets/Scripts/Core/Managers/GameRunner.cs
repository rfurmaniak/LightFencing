using JetBrains.Annotations;
using LightFencing.Core.Interactions;
using LightFencing.Equipment.Armors;
using LightFencing.Equipment.Swords;
using UnityEngine;
using Zenject;

namespace LightFencing.Core.Managers
{
    public class GameRunner : MonoBehaviour
    {
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
            SetupEquipment();
        }

        private void SetupEquipment()
        {
            _sword.AttachToHand(_deviceTransformProvider.GetControllerTransform(Handedness.Right));
            _shield.AttachToHand(_deviceTransformProvider.GetControllerTransform(Handedness.Left));
            _armor.TrackUser(_deviceTransformProvider.GetHeadTransform());
        }
    }
}