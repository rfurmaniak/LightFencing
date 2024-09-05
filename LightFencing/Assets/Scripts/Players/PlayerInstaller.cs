using LightFencing.Players.Controllers;
using UnityEngine;
using Zenject;

namespace LightFencing.Players
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        private Player playerPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<string, bool, IPlayerController, Player, Player.PlayerFactory>().FromComponentInNewPrefab(playerPrefab);
            Container.Bind<LocalPlayerController>().AsSingle();
        }
    }
}
