using JetBrains.Annotations;
using LightFencing.Core.Interactions;
using LightFencing.Players;
using LightFencing.Players.Controllers;
using System;
using Unity.XR.CoreUtils;
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

        [SerializeField]
        private Transform playerASpawnPoint;

        [SerializeField]
        private Transform playerBSpawnPoint;

        private Player.PlayerFactory _playerFactory;
        private LocalPlayerController _playerController;
        private IEnemyController _enemyController;
        private IDeviceTransformProvider _deviceTransformProvider;

        [UsedImplicitly]
        [Inject]
        private void Construct(
            IDeviceTransformProvider deviceTransformProvider,
            LocalPlayerController localPlayerController, 
            IEnemyController enemyController,
            Player.PlayerFactory playerFactory)
        {
            _playerController = localPlayerController;
            _playerFactory = playerFactory;
            _enemyController = enemyController;
            _deviceTransformProvider = deviceTransformProvider;
        }

        private void Start()
        {
            _deviceTransformProvider.Origin.transform.position = playerASpawnPoint.position;
            //_deviceTransformProvider.Origin.MatchOriginUpCameraForward(playerASpawnPoint.up, playerASpawnPoint.forward);
            InitializeLocalPlayer();
            InitializeOpponent();
        }

        private void InitializeLocalPlayer()
        {
            var localPlayer = _playerFactory.Create(Guid.NewGuid().ToString(), true, _playerController);
            localPlayer.transform.SetPositionAndRotation(playerASpawnPoint.position, playerASpawnPoint.rotation);
            localPlayer.Color = playerAColor;
            Player.LocalPlayer = localPlayer;
        }

        private void InitializeOpponent()
        {
            var enemy = _playerFactory.Create(Guid.NewGuid().ToString(), false, _enemyController);
            enemy.transform.SetPositionAndRotation(playerBSpawnPoint.position, playerBSpawnPoint.rotation);
            enemy.Color = playerBColor;
        }
    }
}