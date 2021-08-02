using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using DG.Tweening;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay.PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        public float YToReturnToPool = 25f;
        public float duration = 4f;
        public GameObject effectPrefab;
        public GameObject pickupEffectPrefab;
        public GameObject endEffectPrefab;

        protected virtual void Start()
        {
            transform.DOScale(transform.localScale * 1.3f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

        void Update()
        {
            transform.position += (ManagerGame.instancia.GetVelocidad() / 2) * Vector3.up * Time.deltaTime;

            if (transform.position.y > YToReturnToPool)
            {
                ReturnToPool();
                ManagerGame.instancia.OnPowerUpEnded();
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(Constants.Tags.CHARACTER))
            {
                GameObject pickUpEffect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                Destroy(pickUpEffect, 3f);

                PickUp(col.transform.root);
            }

            ReturnToPool();
        }

        public abstract void PickUp(Transform player);
    
        private void ReturnToPool()
        {
            this.gameObject.SetActive(false);
        }
    }
}
