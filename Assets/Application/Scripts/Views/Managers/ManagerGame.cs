using System.Collections;
using System.Collections.Generic;
using Application.Scripts.Model;
using Application.Scripts.Views.Gameplay;
using Application.Scripts.Views.Gameplay.Obstacles;
using Application.Scripts.Views.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Application.Scripts.Views.Managers
{
    public class ManagerGame : MonoBehaviour
    {
        public static ManagerGame instancia;


        public SceneFader sceneFader;
        public PlayerController player;

        [SerializeField]
        private float velocidadY = 20f;

        public float distanceMultiplier = 0.1f;
        public float velYIncremental = 0.1f;
        public float minPowerUpDelay = 10f;
        public float maxPowerUpDelay = 25f;

        public Text scoreText, coinsText, recordText;
        public GameObject pauseButton;
        public CanvasGroup tutorialPanel;
        public Image deathEffect;

        private float timeSinceGameStarted;
        private bool gameStarted = false;
        private float distance;
        private ObstacleSpawner obstacleSpawner;

        public int Score { get; private set; }
        public int Coins { get; private set; }
        public bool PlayerDied { get; private set; }
        public bool SpawnObstacle { get; set; }
        public bool CanSpawnObstacle { get; set; }


        void Awake()
        {
            instancia = this;
        }

        // Use this for initialization
        void Start()
        {
            CanSpawnObstacle = true;
            SpawnObstacle = false;
            PlayerDied = false;

            Random.InitState(System.DateTime.Now.Millisecond);

            obstacleSpawner = GetComponent<ObstacleSpawner>();
            distance = 0;

            recordText.text = GameSettings.GetMaxScore().ToString();

            AudioManager.instance.StopSound(Constants.Audio.MENU_THEME);
            AudioManager.instance.PlaySound(Constants.Audio.GAME_THEME, true);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale <= 0 || PlayerDied || !gameStarted)
                return;

            velocidadY += velYIncremental * Time.deltaTime;


            distance += Time.deltaTime * velocidadY * distanceMultiplier;

            if (distance >= 1)
            {
                SumarScore();
                distance = 0;
            }

            if (CanSpawnObstacle && SpawnObstacle)
            {
                obstacleSpawner.SpawnObstacle();

                SpawnObstacle = false;
            }
        }

        /// <summary>
        /// Triggered when Start animation of player ends.
        /// </summary>
        public void PlayerInitCompleted()
        {
            if (GameSettings.GetTutorial())
            {
                tutorialPanel.gameObject.SetActive(true);
                tutorialPanel.DOFade(1f, 1.5f).SetEase(Ease.InSine);
                if (GameSettings.GetMusic() == 1)
                {
                    AudioManager.instance.AdjustThemeVolume(0.1f);
                }
            }
            else
            {
                StartCoroutine(StartGame());
            }
        }

        IEnumerator StartGame()
        {
            timeSinceGameStarted = Time.time;

            if (GameSettings.GetMusic() == 1)
            {
                AudioManager.instance.AdjustThemeVolume(0.2f);
            }
            player.enabled = true;
            gameStarted = true;

            yield return new WaitForSeconds(2.5f);

            obstacleSpawner.SpawnObstacle();

            Invoke("SpawnPowerUp", Random.Range(minPowerUpDelay, maxPowerUpDelay));
        }

        public void OnPowerUpEnded()
        {
            Invoke("SpawnPowerUp", Random.Range(minPowerUpDelay, maxPowerUpDelay));
        }

        private void SpawnPowerUp()
        {
            if (!player.HasShield)
            {
                string[] powerUpKeys = new string[] { Constants.PooledObjects.POWERUP_COINIMAN, Constants.PooledObjects.POWERUP_SHIELD, Constants.PooledObjects.POWERUP_SPEEDUP };

                string powerUpKey = powerUpKeys[Random.Range(0, powerUpKeys.Length)];

                GameObject powerUp = ObjectPooler.instance.GetPooledObject(powerUpKey, true);
                powerUp.transform.position = SpawnLocationManager.instance.GetRandomPos(SpawnLocation.Bottom, false);
            }
        }

        public float GetVelocidad()
        {
            return velocidadY;
        }

        public void SetVelocidad(float value)
        {
            velocidadY = value;
        }

        public void SumarScore()
        {
            Score = Score + 1;

            scoreText.text = Score.ToString();
        }

        public void SumarMoneda()
        {
            Coins++;

            coinsText.text = Coins.ToString();
        }

    
        public void LoadScoreScreen()
        {
            sceneFader.FadeTo(Constants.Scenes.SCORE);
        }

        public void TutorialEnded()
        {
            tutorialPanel.gameObject.SetActive(false);
            GameSettings.SetTutorial(false);
            StartCoroutine(StartGame());
        }

        public void OnPlayerDied()
        {
            this.PlayerDied = true;
            deathEffect.color = new Color(deathEffect.color.r, deathEffect.color.g, deathEffect.color.b, 1f);
            deathEffect.DOFade(0f, 0.7f).SetEase(Ease.Linear);

            float gameDuration = Time.time - timeSinceGameStarted;

            Analytics.CustomEvent("Game Stats", new Dictionary<string, object>
            {
                { "Game duration", gameDuration }
            });
        }

        public IEnumerator DestroyObstacles()
        {
            bool anyDestroyed = ObjectPooler.instance.DisableGameObjects(Constants.PooledObjects.ELEVATOR) || 
                                ObjectPooler.instance.DisableGameObjects(Constants.PooledObjects.SECURITY_BOT) ||
                                ObjectPooler.instance.DisableGameObjects(Constants.PooledObjects.OBSTACLE_COINS) ||
                                ObjectPooler.instance.DisableGameObjects(Constants.PooledObjects.RED_PLATFORM);

            yield return new WaitForSeconds(1f);

            if (anyDestroyed)
            {
                SpawnObstacle = true;
            }
        
        }
    }
}
