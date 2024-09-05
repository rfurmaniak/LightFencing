using JetBrains.Annotations;
using LightFencing.Players;
using System;
using UnityEngine;
using Zenject;

namespace LightFencing.Core.Managers
{
    public class GameRunner : MonoBehaviour
    {
        private Player.PlayerFactory _playerFactory;
        private LocalPlayerController _playerController;

        [UsedImplicitly]
        [Inject]
        private void Construct(LocalPlayerController localPlayerController, Player.PlayerFactory playerFactory)
        {
            _playerController = localPlayerController;
            _playerFactory = playerFactory;
        }

        private void Start()
        {
            InitializeLocalPlayer();
        }

        private void InitializeLocalPlayer()
        {
            var localPlayer = _playerFactory.Create(Guid.NewGuid().ToString(), true, _playerController);
            Player.LocalPlayer = localPlayer;
        }
    }
}