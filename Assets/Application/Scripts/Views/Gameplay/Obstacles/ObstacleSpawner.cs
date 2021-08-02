using System.Collections.Generic;
using System.Linq;
using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using Application.Scripts.Views.Utils;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay.Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {

        public ObstacleBGMap[] obstacleBGMaps;
        private SpawnLocationManager spawnLocationManager;

        void Start()
        {
            spawnLocationManager = SpawnLocationManager.instance;
        }

        public void SpawnObstacle()
        {
            ObstacleBGMap obstacleBGMap = obstacleBGMaps.First(x => x.background == BackgroundManager.instance.CurrentBG);

            int obstacleIndex = Random.Range(0, obstacleBGMap.obstacles.Length);
            Model.Obstacles obstacleType = obstacleBGMap.obstacles[obstacleIndex];

            List<int> usedPositions = new List<int>();

            GameObject obstacleGO = GetObstacle(obstacleType);

            Obstacle obstacle = obstacleGO.GetComponent<Obstacle>();

            obstacleGO.transform.position = spawnLocationManager.GetRandomPos(usedPositions, obstacle.spawnLocation, obstacle.onlyCenter, obstacle.YSpawnPos);

            obstacleGO.SetActive(true);

            int currentScore = ManagerGame.instancia.Score;
            if (currentScore < obstacle.minSpawnScore || currentScore > obstacle.maxSpawnScore)
            {
                obstacleGO.SetActive(false);
                SpawnObstacle();
                return;
            }

            if (obstacle.cantMax > 1)
            {
                int cantObstacles = Random.Range(1, obstacle.cantMax + 1);
                for (int i = 1; i < cantObstacles; i++)
                {
                    obstacleGO = GetObstacle(obstacleType);
                    obstacleGO.transform.position = spawnLocationManager.GetRandomPos(usedPositions, obstacle.spawnLocation, obstacle.onlyCenter, obstacle.YSpawnPos);
                    obstacleGO.SetActive(true);
                }
            }
        }

        private GameObject GetObstacle(Model.Obstacles obstacleType)
        {
            GameObject obstacleGO = null;

            switch (obstacleType)
            {
                case Model.Obstacles.RedPlatform:
                    obstacleGO = ObjectPooler.instance.GetPooledObject(Constants.PooledObjects.RED_PLATFORM);
                    break;
                case Model.Obstacles.Elevator:
                    obstacleGO = ObjectPooler.instance.GetPooledObject(Constants.PooledObjects.ELEVATOR);
                    break;
                case Model.Obstacles.SecurityBot:
                    obstacleGO = ObjectPooler.instance.GetPooledObject(Constants.PooledObjects.SECURITY_BOT);
                    obstacleGO.GetComponent<SecurityBot>().InitSecurityBots();
                    break;
                case Model.Obstacles.Coins:
                    obstacleGO = ObjectPooler.instance.GetPooledObject(Constants.PooledObjects.COINS);
                    obstacleGO.GetComponent<CoinsWithObstacle>().EnableRandomGroup();
                    break;
                case Model.Obstacles.ObstacleCoins:
                    obstacleGO = ObjectPooler.instance.GetPooledObject(Constants.PooledObjects.OBSTACLE_COINS);
                    obstacleGO.GetComponent<CoinsWithObstacle>().EnableRandomGroup();
                    break;

            }

            return obstacleGO;
        }



    }
}
