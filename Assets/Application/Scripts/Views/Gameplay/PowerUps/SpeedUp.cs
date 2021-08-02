using UnityEngine;

namespace Application.Scripts.Views.Gameplay.PowerUps
{
    public class SpeedUp : PowerUp
    {
        public float velocity = 80f;

        public override void PickUp(Transform player)
        {
            player.GetComponent<PowerUpHandler>().SpeedUp(this);
        }
    }
}
