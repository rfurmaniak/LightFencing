using UnityEngine;
using Zenject;

namespace LightFencing
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Configs/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private MainConfig mainConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(mainConfig);
        }
    }
}
