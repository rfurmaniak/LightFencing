using UnityEngine;

namespace LightFencing.Players.Controllers
{
    public abstract class AbstractPlayerController
    {
        public abstract Transform HeadTransform { get; }
        public abstract Transform SwordHandTransform { get; }
        public abstract Transform ShieldHandTransform { get; }

        protected Player Player { get; private set; }

        public virtual void Initialize(Player player)
        {
            Player = player;
        }

        public virtual void Clear()
        {

        }
    }
}
