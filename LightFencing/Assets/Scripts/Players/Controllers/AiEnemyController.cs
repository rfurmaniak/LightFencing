using LightFencing.Players;
using LightFencing.Players.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightFencing
{
    public class AiEnemyController : MonoBehaviour, IEnemyController
    {
        [SerializeField]
        private Transform referenceTransform;

        [SerializeField]
        private Transform headTransform;

        [SerializeField]
        private Transform swordHandTransform;

        [SerializeField]
        private Transform shieldHandTransform;

        [SerializeField]
        private Transform shieldHandTarget;

        [SerializeField]
        private Vector3 shieldTargetRotationOffset;

        [SerializeField]
        private float minHandReach;

        [SerializeField]
        private float maxHandReach;

        public Transform HeadTransform => headTransform;

        public Transform SwordHandTransform => swordHandTransform;

        public Transform ShieldHandTransform => shieldHandTransform;

        private Player _thisPlayer;
        private Player _opponent;

        public void Initialize(Player player)
        {
            _thisPlayer = player;
            _opponent = Player.LocalPlayer;

            _opponent.Sword.Activated += OnOpponentSwordActivated;
            _opponent.Sword.Deactivated += OnOpponentSwordDeactivated;
        }

        public void Clear()
        {
            if (_opponent)
            {
                _opponent.Sword.Activated += OnOpponentSwordActivated;
                _opponent.Sword.Deactivated += OnOpponentSwordDeactivated;
            }
        }

        private void FixedUpdate()
        {
            if (_thisPlayer)
                TrackShield();
        }

        private void TrackShield()
        {
            shieldHandTarget.position = _opponent.Sword.SwordTransform.position;
            var direction = _opponent.Sword.SwordTransform.position - shieldHandTransform.position;
            var lookRotation = Quaternion.LookRotation(direction).eulerAngles + shieldTargetRotationOffset;
            shieldHandTarget.rotation = Quaternion.Euler(lookRotation);
        }

        private void OnOpponentSwordDeactivated()
        {
            _thisPlayer.Shield.Deactivate();
        }

        private void OnOpponentSwordActivated()
        {
            _thisPlayer.Shield.Activate();
        }
    }
}
