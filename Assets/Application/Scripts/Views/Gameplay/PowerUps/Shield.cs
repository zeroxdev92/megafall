using UnityEngine;

namespace Application.Scripts.Views.Gameplay.PowerUps
{
    public class Shield : PowerUp
    {
        public override void PickUp(Transform player)
        {
            player.GetComponent<PowerUpHandler>().ActivateShield(this);
        }
    }
}
