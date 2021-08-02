using System.Collections.Generic;
using System.Linq;
using Application.Scripts.Extensions;
using Application.Scripts.Model;
using UnityEngine;

namespace Application.Scripts.Views.Managers
{
    public class SpawnLocationManager : MonoBehaviour
    {
        private const float LeftViewportValue = 0.16666f;
        private const float CenterViewportValue = 0.5f;
        private const float RightViewportValue = 0.83333f;

        #region Singleton
        public static SpawnLocationManager instance;

        void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More than 1 SpawnManager found!");
                return;
            }

            instance = this;
        }

        #endregion

        public Transform bottomSpawnContainer;
        public Transform topSpawnContainer;
        public float offsetY = 15f;

        private Transform[] bottomPositions;
        private Transform[] topPositions;
        private Camera mainCam;

        // Use this for initialization
        void Start()
        {
            mainCam = Camera.main;

            bottomPositions = bottomSpawnContainer.GetChildren();
            topPositions = topSpawnContainer.GetChildren();

            InitSpawnPositions();
        }

        public Vector3 GetRandomPos(SpawnLocation spawnLocation, bool onlyCenter)
        {
            return GetRandomPos(null, spawnLocation, onlyCenter, null);
        }
    
        public Vector3 GetRandomPos(List<int> UsedPositions, SpawnLocation spawnLocation, bool onlyCenter, float? yPos)
        {
            Transform[] positions = spawnLocation == SpawnLocation.Top ? topPositions : bottomPositions;

            Vector3 returnPos = Vector3.zero;

            if (onlyCenter)
            {
                returnPos = positions.First(x => x.name.Contains("Center")).position;
            }
            else
            {
                int nextPosIndex = Random.Range(0, positions.Length);

                if (UsedPositions != null)
                {
                    while (UsedPositions.Contains(nextPosIndex))
                    {
                        nextPosIndex = Random.Range(0, positions.Length);
                    }

                    UsedPositions.Add(nextPosIndex);
                }
            
                returnPos = positions[nextPosIndex].position;
            }


            if (yPos.HasValue)
            {
                returnPos.y = yPos.Value;
            }

            return returnPos;
        }

        public float GetHorizontalPosition(HorizontalPosition location)
        {
            Vector3 position = Vector3.zero;

            switch (location)
            {
                case HorizontalPosition.Left:
                    position = mainCam.ViewportToWorldPoint(Vector3.right * LeftViewportValue);
                    break;
                case HorizontalPosition.Center:
                    position = mainCam.ViewportToWorldPoint(Vector3.right * CenterViewportValue);
                    break;
                case HorizontalPosition.Right:
                    position = mainCam.ViewportToWorldPoint(Vector3.right * RightViewportValue);
                    break;
            }

            return position.x;
        }

        private void InitSpawnPositions()
        {

            for (int i = 0; i < bottomPositions.Length; i++)
            {
                if (bottomPositions[i].name.Contains("Left"))
                {
                    Vector3 leftPos = mainCam.ViewportToWorldPoint(Vector3.right * LeftViewportValue);
                    leftPos.z = 0f;
                    leftPos.y -= offsetY;

                    bottomPositions[i].position = leftPos;
                }
                else if (bottomPositions[i].name.Contains("Center"))
                {
                    Vector3 centerPos = mainCam.ViewportToWorldPoint(Vector3.right * CenterViewportValue);
                    centerPos.z = 0f;
                    centerPos.y -= offsetY;

                    bottomPositions[i].position = centerPos;
                }
                else if (bottomPositions[i].name.Contains("Right"))
                {
                    Vector3 rightPos = mainCam.ViewportToWorldPoint(Vector3.right * RightViewportValue);
                    rightPos.z = 0f;
                    rightPos.y -= offsetY;

                    bottomPositions[i].position = rightPos;
                }
            }



            for (int i = 0; i < topPositions.Length; i++)
            {
                if (topPositions[i].name.Contains("Left"))
                {
                    Vector3 leftPos = mainCam.ViewportToWorldPoint(Vector3.right * 0.16666f);
                    leftPos.z = 0f;
                    leftPos.y = topPositions[i].position.y;

                    topPositions[i].position = leftPos;
                }
                else if (topPositions[i].name.Contains("Center"))
                {
                    Vector3 centerPos = mainCam.ViewportToWorldPoint(Vector3.right * 0.5f);
                    centerPos.z = 0f;
                    centerPos.y = topPositions[i].position.y;

                    topPositions[i].position = centerPos;
                }
                else if (topPositions[i].name.Contains("Right"))
                {
                    Vector3 rightPos = mainCam.ViewportToWorldPoint(Vector3.right * 0.83333f);
                    rightPos.z = 0f;
                    rightPos.y = topPositions[i].position.y;

                    topPositions[i].position = rightPos;
                }
            }


        }
    }
}
