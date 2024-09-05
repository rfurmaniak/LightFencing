using UnityEngine;
using Zenject;

namespace LightFencing.Core.Configs
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Configs/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private MainConfig mainConfig;

        [SerializeField]
        private InputConfig inputConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(mainConfig);
            Container.BindInstance(inputConfig);
        }
    }
}
