using Cysharp.Threading.Tasks;
using LightFencing.Equipment;
using LightFencing.Players;
using LightFencing.Players.Controllers;
using LightFencing.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LightFencing
{
    public class AiEnemyController : MonoBehaviour, IEnemyController
    {
        [SerializeField]
        private TextAsset[] attackMotions;

        [SerializeField]
        private TextAsset dischargeReaction;

        [SerializeField]
        private Transform referenceTransform;

        [SerializeField]
        private Transform headTransform;

        [SerializeField]
        private Transform swordHandTransform;

        [SerializeField]
        private Transform swordHandTarget;

        [SerializeField]
        private Transform shieldHandTransform;

        [SerializeField]
        private Transform shieldHandTarget;

        [SerializeField]
        private Vector3 shieldTargetRotationOffset;

        [SerializeField]
        private Animator animator;

        public Transform HeadTransform => headTransform;

        public Transform SwordHandTransform => swordHandTransform;

        public Transform ShieldHandTransform => shieldHandTransform;

        private Player _thisPlayer;
        private Player _opponent;

        private bool _isAttacking;
        private CancellationTokenSource _attackTokenSource;

        public void Initialize(Player player)
        {
            _attackTokenSource = new CancellationTokenSource();

            _thisPlayer = player;
            _opponent = Player.LocalPlayer;

            _thisPlayer.Sword.SwordDischarged += OnSwordDischarged;

            _opponent.Sword.Activated += OnOpponentSwordActivated;
            _opponent.Sword.Deactivated += OnOpponentSwordDeactivated;
        }

        public void Clear()
        {
            if (_thisPlayer)
            {
                _thisPlayer.Sword.SwordDischarged -= OnSwordDischarged;
            }
            if (_opponent)
            {
                _opponent.Sword.Activated += OnOpponentSwordActivated;
                _opponent.Sword.Deactivated += OnOpponentSwordDeactivated;
            }
            _attackTokenSource.Cancel();
            _attackTokenSource.Dispose();
        }

        private void FixedUpdate()
        {
            if (!_thisPlayer)
                return;

            TrackShield();
            if (!_isAttacking && !_thisPlayer.Sword.IsSwordDischarged)
                PerformRandomAttack(_attackTokenSource.Token).Forget();
        }

        private void TrackShield()
        {          
            shieldHandTarget.position = _opponent.Sword.SwordTransform.position;
          //  var direction = _opponent.Sword.SwordTransform.position - shieldHandTransform.position;
          //  var lookRotation = Quaternion.LookRotation(direction).eulerAngles + shieldTargetRotationOffset;
          //  shieldHandTarget.rotation = Quaternion.Euler(lookRotation);
        }

        private void OnOpponentSwordDeactivated()
        {
            _thisPlayer.Shield.Deactivate();
        }

        private void OnOpponentSwordActivated()
        {
            _thisPlayer.Shield.Activate();
        }

        private void OnSwordDischarged()
        {
            _attackTokenSource.Cancel();
            _attackTokenSource.Dispose();
            _attackTokenSource = new CancellationTokenSource();

            PerformAction(dischargeReaction, swordHandTarget, 0.05f, false, _attackTokenSource.Token).Forget();
        }

        private async UniTask PerformRandomAttack(CancellationToken cancellationToken)
        {
            _isAttacking = true;
            var randomIndex = UnityEngine.Random.Range(0, attackMotions.Length);
            var motion = attackMotions[randomIndex];
            await PerformAction(motion, swordHandTarget, 0.1f, true, cancellationToken);
            _isAttacking = false;
        }

        private async UniTask PerformAction(TextAsset motion, Transform transformToMove, float lerpTimes, bool returnToStart, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            var frames = JsonConvert.DeserializeObject<List<MovementFrame>>(motion.text);
            await PerformAction(frames, transformToMove, lerpTimes, returnToStart, cancellationToken);
        }

        private async UniTask PerformAction(List<MovementFrame> frames, Transform transformToMove, float lerpTimes, bool returnToStart, CancellationToken cancellationToken)
        {
            var startPosition = transformToMove.position;
            var startRotation = transformToMove.rotation;

            var (initialPosition, initialRotation) = GetTransformFromFrame(frames[0]);
            await LerpUtils.LerpTo(transformToMove, initialPosition, initialRotation, lerpTimes);
            foreach (var currentFrame in frames)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var (position, rotation) = GetTransformFromFrame(currentFrame);
                transformToMove.SetPositionAndRotation(position, rotation);

                ParseActions(currentFrame.Actions);

                await UniTask.Yield();
            }

            if (transformToMove && returnToStart)
                await LerpUtils.LerpTo(transformToMove, startPosition, startRotation, lerpTimes);
        }

        private (Vector3 position, Quaternion rotation) GetTransformFromFrame(MovementFrame frame)
        {
            return (referenceTransform.TransformPoint(frame.RightHandPosition),
                    referenceTransform.rotation * Quaternion.Euler(frame.RightHandRotation));
        }

        private void ParseActions(EquipmentAction action)
        {
            if (action == EquipmentAction.None)
                return;

            if (action.HasFlag(EquipmentAction.SwordActivated))
                _thisPlayer.Sword.Activate();
            if (action.HasFlag(EquipmentAction.SwordDeactivated))
                _thisPlayer.Sword.Deactivate();
        }
    }
}
