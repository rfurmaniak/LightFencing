using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace LightFencing.Players.Controllers
{
    public class StaticPlayerController : MonoBehaviour, IEnemyController
    {
        [SerializeField]
        private Transform headTransform;

        [SerializeField]
        private Transform swordHandTransform;

        [SerializeField]
        private Transform shieldHandTransform;

        [SerializeField]
        private float swordActivatedTimeInSeconds;

        [SerializeField]
        private float swordDeactivatedTimeInSeconds;

        [SerializeField]
        private float shieldActivatedTimeInSeconds;

        [SerializeField]
        private float shieldDeactivatedTimeInSeconds;

        public Transform HeadTransform => headTransform;

        public Transform SwordHandTransform => swordHandTransform;

        public Transform ShieldHandTransform => shieldHandTransform;

        private CancellationTokenSource _tokenSource;
        private Player _player;

        public void Initialize(Player player)
        {
            _player = player;
            _tokenSource = new CancellationTokenSource();
            VeryBasicSwordBehavior().Forget();
            VeryBasicShieldBehavior().Forget();
        }

        public void Clear()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        private async UniTask VeryBasicSwordBehavior()
        {
            while (true)
            {
                if (_tokenSource.IsCancellationRequested)
                    return;
                _player.Sword.Activate();
                await UniTask.Delay((int)(swordActivatedTimeInSeconds * 1000));
                if (_tokenSource.IsCancellationRequested)
                    return;
                _player.Sword.Deactivate();
                await UniTask.Delay((int)(swordDeactivatedTimeInSeconds * 1000));
            }
        }

        private async UniTask VeryBasicShieldBehavior()
        {
            while (true)
            {
                if (_tokenSource.IsCancellationRequested)
                    return;
                _player.Shield.Activate();
                await UniTask.Delay((int)(shieldActivatedTimeInSeconds * 1000));
                if (_tokenSource.IsCancellationRequested)
                    return;
                _player.Shield.Deactivate();
                await UniTask.Delay((int)(shieldDeactivatedTimeInSeconds * 1000));
            }
        }
    }
}
