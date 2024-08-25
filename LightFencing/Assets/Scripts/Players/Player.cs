using UnityEngine;

namespace LightFencing.Players
{
    public class Player : MonoBehaviour
    {
        public static Player LocalPlayer { get; set; }
        public string Id { get; set; }
    }
}
