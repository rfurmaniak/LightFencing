using JetBrains.Annotations;
using LightFencing.Core.Configs;
using LightFencing.Players;
using System;
using UnityEngine;
using Zenject;

namespace LightFencing.Core.Managers
{
    public class GameRunner : MonoBehaviour
    {
        private Player.PlayerFactory _playerFactory;
        private InputConfig _inputConfig;

        [UsedImplicitly]
        [Inject]
        private void Construct(InputConfig inputConfig, Player.PlayerFactory playerFactory)
        {
            _inputConfig = inputConfig;
            _playerFactory = playerFactory;
        }

        private void Start()
        {
            InitializeLocalPlayer();
        }

        private void InitializeLocalPlayer()
        {
            var controller = new LocalPlayerController(_inputConfig.ActivateSwordAction, _inputConfig.ActivateShieldAction);
            var localPlayer = _playerFactory.Create(Guid.NewGuid().ToString(), true, controller);
            Player.LocalPlayer = localPlayer;
        }
    }
}