using UnityEngine;

namespace LightFencing.Players.Controllers
{
    public interface IPlayerController
    {
        Transform HeadTransform { get; }
        Transform SwordHandTransform { get; }
        Transform ShieldHandTransform { get; }

        void Initialize(Player player);

        void Clear();
    }
}
