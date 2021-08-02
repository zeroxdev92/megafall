using UnityEngine;

namespace Application.Scripts.Views.Gameplay.PowerUps
{
    public class CoinIman : PowerUp
    {
        public float height = 7f;
        public LayerMask mask;
        public override void PickUp(Transform player)
        {
            player.GetComponent<PowerUpHandler>().ActivateCoinIman(this);
        }
    }
}
