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

        [UsedImplicitly]
        [Inject]
        private void Construct(Player.PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        private void Start()
        {
            InitializeLocalPlayer();
        }

        private void InitializeLocalPlayer()
        {
            var localPlayer = _playerFactory.Create(Guid.NewGuid().ToString(), true);
            Player.LocalPlayer = localPlayer;
        }
    }
}