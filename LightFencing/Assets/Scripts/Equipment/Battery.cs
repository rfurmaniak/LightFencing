using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LightFencing.Core.Configs;
using System;
using System.Threading;
using Unity.XR.CoreUtils;
using UnityEngine;
using Zenject;

namespace LightFencing
{
    public class Battery : MonoBehaviour
    {
        public event Action BatteryDrained;

        private int _batteryDrainSpeed;
        private int _batteryRechargeSpeed;
        private int _millisecondsInterval;
        private int _batteryDrainedTime;
        private int _numberOfBatteryDrainers;

        private CancellationTokenSource _cancellationTokenSource;
        private readonly object _batteryLock = new();

        public int MaxBatteryLevel { get; private set; }
        public int CurrentBatteryLevel { get; private set; }

        public bool IsBatteryUsed { get; private set; }
        public bool IsBatteryDrained { get; private set; }
        public bool IsPaused { get; set; }

        [UsedImplicitly]
        [Inject]
        private void Construct(MainConfig mainConfig)
        {
            CurrentBatteryLevel = MaxBatteryLevel = mainConfig.MaxBatteryLevel;
            _batteryDrainSpeed = mainConfig.BatteryDrainSpeed;
            _batteryRechargeSpeed = mainConfig.BatteryRechargeSpeed;
            _millisecondsInterval = mainConfig.BatteryUpdateMillisecondsInterval;
            _batteryDrainedTime = mainConfig.BatteryDrainedMillisecondsTime;
            _cancellationTokenSource = new CancellationTokenSource();
            HandleBatteryLevel(_cancellationTokenSource.Token).Forget();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask HandleBatteryLevel(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (!IsPaused && !IsBatteryDrained)
                {
                    if (IsBatteryUsed)
                    {
                        CurrentBatteryLevel = Mathf.Clamp(CurrentBatteryLevel - _batteryDrainSpeed, 0, MaxBatteryLevel);
                        if (CurrentBatteryLevel == 0)
                        {
                            HandleBatteryDrained();
                        }
                    }
                    else
                    {
                        CurrentBatteryLevel = Mathf.Clamp(CurrentBatteryLevel + _batteryRechargeSpeed, 0, MaxBatteryLevel);
                    }
                }

                await UniTask.Delay(_millisecondsInterval);
            }
        }

        public void StartUsingBattery()
        {
            lock (_batteryLock)
            {
                Mathf.Clamp(_numberOfBatteryDrainers + 1, 0, 2);
                IsBatteryUsed = true;
            }
        }

        public void StopUsingBattery()
        {
            lock (_batteryLock)
            {
                _numberOfBatteryDrainers = Mathf.Clamp(_numberOfBatteryDrainers - 1, 0, 2);
                if (_numberOfBatteryDrainers == 0)
                    IsBatteryUsed = false;
            }
        }

        private void HandleBatteryDrained()
        {
            IsBatteryDrained = true;
            BatteryDrained?.Invoke();

            BatteryDrainCooldown(_cancellationTokenSource.Token).Forget();
        }

        private async UniTask BatteryDrainCooldown(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(_batteryDrainedTime, cancellationToken: cancellationToken);
            }
            catch (Exception _)
            {
                Debug.Log("Battery drain regain cancelled");
                return;
            }
            IsBatteryDrained = false;
        }
    }
}
