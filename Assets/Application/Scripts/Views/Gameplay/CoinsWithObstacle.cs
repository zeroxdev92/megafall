using System.Collections.Generic;
using Application.Scripts.Views.Gameplay.Obstacles;
using Application.Scripts.Views.Managers;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay
{
    public class CoinsWithObstacle : Obstacle
    {

        private List<int> prevIndexList = new List<int>();

        public override void Update()
        {
            transform.position += ManagerGame.instancia.GetVelocidad() * Vector3.up * Time.deltaTime;

            if (transform.position.y > YToReturnToPool)
            {
                ReturnToPool();
            }
        }

        public override void ReturnToPool()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform group = transform.GetChild(i);

                for (int y = 0; y < transform.GetChild(i).childCount; y++)
                {
                    group.GetChild(y).gameObject.SetActive(true);
                }

                group.gameObject.SetActive(false);
            }

            base.ReturnToPool();
        }

        public void ReturnToPoolPreventSpawnTrigger()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform group = transform.GetChild(i);

                for (int y = 0; y < transform.GetChild(i).childCount; y++)
                {
                    group.GetChild(y).gameObject.SetActive(true);
                }

                group.gameObject.SetActive(false);
            }

            this.gameObject.SetActive(false);
        }

        public void EnableRandomGroup()
        {
            if (prevIndexList.Count >= 4)
                prevIndexList.RemoveAt(0);

            int childIndex = Random.Range(0, transform.childCount);

            while (prevIndexList.Contains(childIndex))
            {
                childIndex = Random.Range(0, transform.childCount);
            }

            transform.GetChild(childIndex).gameObject.SetActive(true);
            prevIndexList.Add(childIndex);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, new Vector3(transform.right.x * 50f, transform.position.y, transform.position.z));
            Gizmos.DrawLine(transform.position, new Vector3(-transform.right.x * 50f, transform.position.y, transform.position.z));
        }
    }
}
