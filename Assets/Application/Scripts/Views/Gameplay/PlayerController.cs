using System.Collections;
using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        public float borderMargin = 0.8f;
        public float verticalDashDuration = 0.5f;
        public float horizontalDashDuration = 0.3f;
        public Animator animator;
        public ParticleSystem[] particleHandEffects;
        public Transform hipsBone;
        public GameObject shieldEffect, shieldEndEfectPrefab, magnetEndEffect;

        private Rigidbody rb;
        private Camera mainCam;
        private ManagerGame gm;
        private TouchController touchController;
        private bool canDashDown = true;
        private bool canDashUp = true;
        private float yPos = 1f;
        private float leftXPos, centerXPos, rightXPos;

        private bool _hasShield = false;

        public bool HasShield
        {
            get => _hasShield;
            set
            {
                _hasShield = value;
                shieldEffect.SetActive(value);

                if (!value)
                {
                    GameObject endEffect = Instantiate(shieldEndEfectPrefab, transform.position, Quaternion.identity);
                    Destroy(endEffect, 3f);

                    gm.OnPowerUpEnded();
                }
            }
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            touchController = GetComponent<TouchController>();
        }

        void OnEnable()
        {
            yPos = transform.position.y;
            touchController.OnSwipe += HandleInput;
        }

        void OnDisable()
        {
            touchController.OnSwipe -= HandleInput;
        }

        // Use this for initialization
        void Start()
        {
            mainCam = Camera.main;
            gm = ManagerGame.instancia;
            leftXPos = SpawnLocationManager.instance.GetHorizontalPosition(HorizontalPosition.Left);
            centerXPos = SpawnLocationManager.instance.GetHorizontalPosition(HorizontalPosition.Center);
            rightXPos = SpawnLocationManager.instance.GetHorizontalPosition(HorizontalPosition.Right);
        }

        // Update is called once per frame
        void Update()
        {
            if (gm.PlayerDied || Time.timeScale <= 0)
                return;

#if UNITY_EDITOR
            HandleMovement();
#endif
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.layer == Constants.Layers.OBSTACLE)
            {
                if (HasShield)
                {
                    HasShield = false;
                }
                else
                {
                    StartCoroutine(Die());
                }
            }
            else if (col.gameObject.layer == Constants.Layers.SIMPLE_COIN)
            {
                Coin coin = col.GetComponent<Coin>();

                coin.OnPickUp();
            }
            else if (col.CompareTag(Constants.Tags.DOOR_TRIGGER))
            {
                col.transform.parent.GetComponent<Animator>().SetTrigger(Constants.AnimationParams.OPEN_DOOR);

                gm.StartCoroutine(gm.DestroyObstacles());
            }
        }

        private void HandleMovement()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                HandleInput(SwipeDirection.Left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                HandleInput(SwipeDirection.Right);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                HandleInput(SwipeDirection.Up);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                HandleInput(SwipeDirection.Down);
            }
        }

        private void HandleInput(SwipeDirection swipeDir)
        {
            transform.localRotation = Quaternion.identity;

            Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);

            switch (swipeDir)
            {
                case SwipeDirection.Up:

                    if (!canDashUp)
                        return;

                    animator.SetTrigger(Constants.AnimationParams.ROLL_UP);

                    canDashUp = false;

                    DOTween.Sequence().Append(transform.DOMoveY(11f, verticalDashDuration, false))
                        .Append(transform.DOMoveY(yPos, verticalDashDuration, false))
                        .OnKill(() => { canDashUp = true; });


                    break;
                case SwipeDirection.Left:

                    animator.SetTrigger(Constants.AnimationParams.DASH_LEFT);

                    if (viewportPos.x <= 0.15f) // esta en el borde izq no dasheo
                        return;

                    if (viewportPos.x >= 0.6f) //carril der a centro
                    {
                        transform.DOMoveX(centerXPos, horizontalDashDuration, false);
                    }
                    else if (viewportPos.x < 0.6f && viewportPos.x >= 0.3f) // carril del centro a izq
                    {
                        transform.DOMoveX(leftXPos, horizontalDashDuration, false);
                    }
                    else if (viewportPos.x < 0.3f)
                    {
                        float toPosX = leftXPos - borderMargin;

                        transform.DOMoveX(toPosX, horizontalDashDuration, false);
                    }

                    break;
                case SwipeDirection.Right:

                    animator.SetTrigger(Constants.AnimationParams.DASH_RIGHT);

                    if (viewportPos.x > 0.85f) //esta en el borde der no dasheo.
                        return;


                    if (viewportPos.x <= 0.3f) //carril izq a centro
                    {
                        transform.DOMoveX(centerXPos, horizontalDashDuration, false);
                    }
                    else if (viewportPos.x > 0.3f && viewportPos.x <= 0.6f) // carril del centro a derecho
                    {
                        transform.DOMoveX(rightXPos, horizontalDashDuration, false);
                    }
                    else if (viewportPos.x > 0.6f)
                    {
                        float toPosX = rightXPos + borderMargin;

                        transform.DOMoveX(toPosX, horizontalDashDuration, false);
                    }

                    break;
                case SwipeDirection.Down:

                    if (!canDashDown)
                        return;

                    animator.SetTrigger(Constants.AnimationParams.DOWN_FALL);

                    canDashDown = false;

                    DOTween.Sequence().Append(transform.DOMoveY(-11f, verticalDashDuration, false))
                        .Append(transform.DOMoveY(yPos, verticalDashDuration, false))
                        .OnKill(() => { canDashDown = true; });

                    break;
            }
        }

        IEnumerator Die()
        {
            transform.position = hipsBone.position;
            hipsBone.GetComponent<CapsuleCollider>().enabled = false;

            gm.OnPlayerDied();
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            animator.SetTrigger(Constants.AnimationParams.DIE);

            for (int i = 0; i < particleHandEffects.Length; i++)
            {
                particleHandEffects[i].gameObject.SetActive(false);
            }

            Transform currentEffect = transform.GetChild(transform.childCount - 1);
            if (currentEffect != null && currentEffect.name.Contains("Magnet Effect"))
            {
                Destroy(currentEffect.gameObject);
                Instantiate(magnetEndEffect, transform.position,
                    Quaternion.identity); //No lo destruyo porq se destruye al cambiar la escena.
            }


            this.enabled = false;

            GameSettings.SetScore(gm.Score);
            GameSettings.SetGameCoins(gm.Coins);

            int totalCoins = GameSettings.GetCoins();
            GameSettings.SetCoins(gm.Coins + totalCoins);

            while (transform.position.y > -14f)
            {
                yield return null;
            }

            gm.pauseButton.SetActive(false);
            gm.LoadScoreScreen();

            gameObject.SetActive(false);
        }

        [UsedImplicitly]
        public void StartAnimationEnded()
        {
            ManagerGame.instancia.PlayerInitCompleted();
        }
    }
}