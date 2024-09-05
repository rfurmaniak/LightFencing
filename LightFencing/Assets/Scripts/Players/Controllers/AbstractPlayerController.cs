namespace LightFencing.Players.Controllers
{
    public abstract class AbstractPlayerController
    {
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
