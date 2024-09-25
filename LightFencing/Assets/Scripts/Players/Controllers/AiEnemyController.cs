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
        private float minReactionSpeed = 0.1f;

        [SerializeField]
        private float maxReactionSpeed = 0.5f;

        [SerializeField]
        private float idleReactionSpeed = 0.05f;

        [SerializeField]
        private float maxTimeBetweenAttacks = 3.0f;

        [SerializeField]
        private float frameAttackChance = 0.01f;

        [SerializeField]
        private float dischargeReactionTime = 0.05f;

        [SerializeField]
        private float panicDistance = 0.3f;

        [SerializeField]
        private float panicChance = 0.25f;

        [SerializeField]
        private float panicTime = 0.5f;

        [SerializeField]
        private float panicSpeedMultiplier = 2.0f;

        [SerializeField]
        private float chanceToStopTheAttack = 0.1f;

        [SerializeField]
        private float minDefenseDelay = 0.1f;

        [SerializeField]
        private float maxDefenseDelay = 0.5f;

        [SerializeField]
        private Animator animator;

        private Player _thisPlayer;
        private Player _opponent;

        private float _currentReactionSpeed;
        private float _lastAttackEnd;

        private Vector3 _lastOpponentSwordPosition;

        private bool _isAttacking;

        private CancellationTokenSource _attackTokenSource;

        public Transform HeadTransform => headTransform;

        public Transform SwordHandTransform => swordHandTransform;

        public Transform ShieldHandTransform => shieldHandTransform;
        private bool IsBlocking => _thisPlayer.Shield.IsActivated;

        public void Initialize(Player player)
        {
            _attackTokenSource = new CancellationTokenSource();

            _thisPlayer = player;
            _opponent = Player.LocalPlayer;

            _thisPlayer.Sword.SwordDischarged += OnSwordDischarged;

            _opponent.Sword.Activated += OnOpponentSwordActivated;
            _opponent.Sword.Deactivated += OnOpponentSwordDeactivated;

            _opponent.Shield.Activated += OnOpponentShieldActivated;

            _lastOpponentSwordPosition = _opponent.Sword.SwordTransform.position;

            RefreshReactionSpeed(true);
        }

        public void Clear()
        {
            if (_thisPlayer)
            {
                _thisPlayer.Sword.SwordDischarged -= OnSwordDischarged;
            }
            if (_opponent)
            {
                _opponent.Sword.Activated -= OnOpponentSwordActivated;
                _opponent.Sword.Deactivated -= OnOpponentSwordDeactivated;
                _opponent.Shield.Activated -= OnOpponentShieldActivated;
            }
            _attackTokenSource.Cancel();
            _attackTokenSource.Dispose();
        }

        private void Update()
        {
            if (!_opponent)
                return;
            var target = _opponent.Armor.BodyTransform.position;
            var lookAtTarget = new Vector3(target.x, transform.position.y, target.z);
            transform.LookAt(lookAtTarget);
        }

        private void FixedUpdate()
        {
            if (!_thisPlayer)
                return;

            TrackShield();
            if (!_isAttacking && !IsBlocking && !_thisPlayer.Sword.IsSwordDischarged && RollForAttack())
                PerformRandomAttack(_attackTokenSource.Token).Forget();
        }

        private void TrackShield()
        {
            var opponentSwordPosition = _opponent.Sword.SwordTransform.position;
            shieldHandTarget.position = Vector3.MoveTowards(shieldHandTarget.position, opponentSwordPosition, _currentReactionSpeed);
            if (Vector3.Distance(_lastOpponentSwordPosition, opponentSwordPosition) >= panicDistance && Random.Range(0.0f, 1.0f) < panicChance)
                PanicShielding().Forget();
            _lastOpponentSwordPosition = opponentSwordPosition;
        }

        private async void OnOpponentSwordDeactivated()
        {
            await UniTask.Delay((int)(Random.Range(minDefenseDelay, maxDefenseDelay) * 1000));
            _thisPlayer.Shield.Deactivate();
            RefreshReactionSpeed(true);
        }

        private async void OnOpponentSwordActivated()
        {
            await UniTask.Delay((int)(Random.Range(minDefenseDelay, maxDefenseDelay) * 1000));
            _thisPlayer.Shield.Activate();
            RefreshReactionSpeed(false);
        }

        private void OnOpponentShieldActivated()
        {
            if (_isAttacking && Random.Range(0.0f, 1.0f) < chanceToStopTheAttack)
                CancelAndResetAttackTokenSource();
        }

        private void OnSwordDischarged()
        {
            CancelAndResetAttackTokenSource();

            PerformAction(dischargeReaction, swordHandTarget, BodyPart.RightHand, dischargeReactionTime, false, _attackTokenSource.Token).Forget();
        }

        private bool RollForAttack()
        {
            var diff = Time.time - _lastAttackEnd;
            if (maxTimeBetweenAttacks <= diff)
                return true;
            return Random.Range(0.0f, 1.0f) < frameAttackChance;
        }

        private async UniTask PanicShielding()
        {
            _thisPlayer.Shield.Activate();
            _currentReactionSpeed *= panicSpeedMultiplier;
            await UniTask.Delay((int)(panicTime * 1000));
            _thisPlayer.Shield.Deactivate();
            RefreshReactionSpeed(_opponent.Sword.IsActivated);
        }

        private async UniTask PerformRandomAttack(CancellationToken cancellationToken)
        {
            _isAttacking = true;
            var randomIndex = UnityEngine.Random.Range(0, attackMotions.Length);
            var motion = attackMotions[randomIndex];
            await PerformAction(motion, swordHandTarget, BodyPart.RightHand, 0.1f, true, cancellationToken);
            _thisPlayer.Sword.Deactivate();
            _isAttacking = false;
            _lastAttackEnd = Time.time;
        }

        private async UniTask PerformAction(TextAsset motion, Transform transformToMove, BodyPart bodyPartToTrack, float lerpTimes, bool returnToStart, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            var frames = JsonConvert.DeserializeObject<List<MovementFrame>>(motion.text);
            await PerformAction(frames, transformToMove, bodyPartToTrack, lerpTimes, returnToStart, cancellationToken);
        }

        private async UniTask PerformAction(List<MovementFrame> frames, Transform transformToMove, BodyPart bodyPartToTrack, float lerpTimes, bool returnToStart, CancellationToken cancellationToken)
        {
            var startPosition = transformToMove.position;
            var startRotation = transformToMove.rotation;

            var (initialPosition, initialRotation) = GetTransformFromFrame(frames[0], bodyPartToTrack);
            await LerpUtils.LerpTo(transformToMove, initialPosition, initialRotation, lerpTimes);
            foreach (var currentFrame in frames)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var (position, rotation) = GetTransformFromFrame(currentFrame, bodyPartToTrack);
                transformToMove.SetPositionAndRotation(position, rotation);

                ParseActions(currentFrame.Actions);

                await UniTask.Yield();
            }

            if (transformToMove && returnToStart)
                await LerpUtils.LerpTo(transformToMove, startPosition, startRotation, lerpTimes);
        }

        private (Vector3 position, Quaternion rotation) GetTransformFromFrame(MovementFrame frame, BodyPart bodyPart)
        {
            return bodyPart switch
            {
                BodyPart.None => (Vector3.zero, Quaternion.identity),
                BodyPart.RightHand => (referenceTransform.TransformPoint(frame.RightHandPosition),
                    referenceTransform.rotation * Quaternion.Euler(frame.RightHandRotation)),
                BodyPart.LeftHand => (referenceTransform.TransformPoint(frame.LeftHandPosition),
                    referenceTransform.rotation * Quaternion.Euler(frame.LeftHandRotation)),
                BodyPart.Head => (referenceTransform.TransformPoint(frame.HeadPosition),
                    referenceTransform.rotation * Quaternion.Euler(frame.HeadRotation)),
                BodyPart.Body => (referenceTransform.TransformPoint(frame.BodyPosition),
                    referenceTransform.rotation * Quaternion.Euler(frame.BodyRotation)),
                _ => throw new System.NotImplementedException(),
            };
        }

        private void ParseActions(EquipmentAction action, bool parseSwordDeactivation = false)
        {
            if (action == EquipmentAction.None)
                return;

            if (action.HasFlag(EquipmentAction.SwordActivated))
                _thisPlayer.Sword.Activate();
            if (parseSwordDeactivation && action.HasFlag(EquipmentAction.SwordDeactivated))
                _thisPlayer.Sword.Deactivate();
        }

        private void RefreshReactionSpeed(bool isIdle)
        {
            _currentReactionSpeed = isIdle ? idleReactionSpeed : Random.Range(minReactionSpeed, maxReactionSpeed);
        }

        private void CancelAndResetAttackTokenSource()
        {
            _attackTokenSource.Cancel();
            _attackTokenSource.Dispose();
            _attackTokenSource = new CancellationTokenSource();
        }

    }
}
