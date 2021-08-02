using System.Collections;
using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Scripts.Views.Gameplay.PowerUps
{
    public class PowerUpHandler : MonoBehaviour
    {

        private PlayerController playerController;
        private Animator playerAnim;
        private ManagerGame gm;

        private Coroutine shieldCoroutine;
        public Image shieldUIEffect;

        private Coroutine coinImanCoroutine;
        public Image imanUIEffect;
        private GameObject imanEffect;

        public Image speedUpUIEffect;
        private Coroutine speedUpCoroutine;
        private GameObject speedUpEffect;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();
            playerAnim = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            gm = ManagerGame.instancia;
        }


        public void ActivateShield(Shield shield)
        {
            if (shieldCoroutine != null)
                StopCoroutine(shieldCoroutine);


            shieldCoroutine = StartCoroutine(DoActivateShield(shield));
        }

        IEnumerator DoActivateShield(Shield shield)
        {
            shieldUIEffect.color = new Color(shieldUIEffect.color.r, shieldUIEffect.color.g, shieldUIEffect.color.b, 1f);
            shieldUIEffect.DOFade(0f, 0.3f).SetEase(Ease.Linear);
            playerController.HasShield = true;

            yield return new WaitForSeconds(shield.duration);

            if (playerController.HasShield)
            {
                playerController.HasShield = false;
            }
        
        }

        public void ActivateCoinIman(CoinIman coinIman)
        {
            if (coinImanCoroutine != null)
                StopCoroutine(coinImanCoroutine);


            coinImanCoroutine = StartCoroutine(DoActivateCoinIman(coinIman));
        }

        IEnumerator DoActivateCoinIman(CoinIman coinIman)
        {
            if (imanEffect == null)
            {
                imanEffect = Instantiate(coinIman.effectPrefab, playerController.transform);
            }

            imanUIEffect.color = new Color(imanUIEffect.color.r, imanUIEffect.color.g, imanUIEffect.color.b, 1f);
            imanUIEffect.DOFade(0f, 0.3f).SetEase(Ease.Linear);
        
            float width = (float)(Camera.main.orthographicSize * 2.0 * Screen.width / Screen.height);

            float elapsed = 0f;

            while (elapsed <= coinIman.duration)
            {

                Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(width, coinIman.height), Quaternion.identity, coinIman.mask);  //Physics.OverlapSphere(transform.position, coinIman.radius, coinIman.mask);

                for (int i = 0; i < colliders.Length; i++)
                {
                    Coin coin = colliders[i].GetComponent<Coin>();

                    if (coin != null)
                    {
                        coin.targetToChase = playerController.transform;
                    }
                }

                elapsed += Time.deltaTime;
                yield return null;
            }


            Destroy(imanEffect);
            imanEffect = null;
            SpawnEndEffect(coinIman);
        }


        public void SpeedUp(SpeedUp speedUp)
        {
            if (speedUpCoroutine != null)
                StopCoroutine(speedUpCoroutine);


            speedUpCoroutine = StartCoroutine(DoSpeedUp(speedUp));
        }

        IEnumerator DoSpeedUp(SpeedUp speedUp)
        {
            speedUpUIEffect.color = new Color(speedUpUIEffect.color.r, speedUpUIEffect.color.g, speedUpUIEffect.color.b, 1f);
            speedUpUIEffect.DOFade(0f, 0.3f).SetEase(Ease.Linear);

            gm.CanSpawnObstacle = false;

            gm.StartCoroutine(gm.DestroyObstacles());

            playerAnim.SetTrigger(Constants.AnimationParams.SPEED_UP_START);

            if (speedUpEffect == null)
            {
                speedUpEffect = Instantiate(speedUp.effectPrefab, playerController.transform);
            }
        

            float currentVelocity = gm.GetVelocidad();

            CapsuleCollider collider = GetComponentInChildren<CapsuleCollider>();

            playerController.enabled = false;
            collider.enabled = false;
            gm.SetVelocidad(currentVelocity + speedUp.velocity);
        
            yield return new WaitForSeconds(speedUp.duration);

            playerAnim.SetTrigger(Constants.AnimationParams.SPEED_UP_EXIT);
            collider.enabled = true;
            playerController.enabled = true;
            gm.SetVelocidad(currentVelocity);

            Destroy(speedUpEffect);
            speedUpEffect = null;

            SpawnEndEffect(speedUp);

            yield return new WaitForSeconds(1f);

            gm.CanSpawnObstacle = true;

        }

        private void SpawnEndEffect(PowerUp powerUp)
        {
            GameObject endEffect = Instantiate(powerUp.endEffectPrefab, playerController.transform.position, Quaternion.identity);
            Destroy(endEffect, 3f);
            gm.OnPowerUpEnded();
        }
    }
}
