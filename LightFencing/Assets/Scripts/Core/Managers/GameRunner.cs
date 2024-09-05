using JetBrains.Annotations;
using LightFencing.Players;
using LightFencing.Players.Controllers;
using System;
using UnityEngine;
using Zenject;

namespace LightFencing.Core.Managers
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField]
        private Color playerAColor;

        [SerializeField]
        private Color playerBColor;

        private Player.PlayerFactory _playerFactory;
        private LocalPlayerController _playerController;
        private IEnemyController _enemyController;

        [UsedImplicitly]
        [Inject]
        private void Construct(LocalPlayerController localPlayerController, IEnemyController enemyController, Player.PlayerFactory playerFactory)
        {
            _playerController = localPlayerController;
            _playerFactory = playerFactory;
            _enemyController = enemyController;
        }

        private void Start()
        {
            InitializeLocalPlayer();
            InitializeOpponent();
        }

        private void InitializeLocalPlayer()
        {
            var localPlayer = _playerFactory.Create(Guid.NewGuid().ToString(), true, _playerController);
            localPlayer.Color = playerAColor;
            Player.LocalPlayer = localPlayer;
        }

        private void InitializeOpponent()
        {
            var enemy = _playerFactory.Create(Guid.NewGuid().ToString(), false, _enemyController);
            enemy.Color = playerBColor;
        }
    }
}