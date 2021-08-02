using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        public SpawnLocation spawnLocation;
        public bool onlyCenter = false;
        public int cantMax = 1;
        public float YSpawnPos = -30f;
        public GameObject destroyedVersion;
        public float YToReturnToPool = 25f;
        public bool triggerNewObstacle = true;
        [Range(0, 1000000000)]
        public int minSpawnScore = 0;
        [Range(0, 1000000000)]
        public int maxSpawnScore = 1000000000;

        protected bool firstInstantiation = true;
        private float speed;

        // Update is called once per frame
        public virtual void Update()
        {
            if (spawnLocation == SpawnLocation.Top)
                return;

            transform.position += ManagerGame.instancia.GetVelocidad() * Vector3.up * Time.deltaTime;

            if (transform.position.y > YToReturnToPool)
            {
                ReturnToPool();
            }
        }

        public virtual void ReturnToPool()
        {
            this.gameObject.SetActive(false);

            if (triggerNewObstacle)
            {
                ManagerGame.instancia.SpawnObstacle = true;
            }
        }

        protected virtual void OnDisable()
        {
            if (firstInstantiation)
            {
                firstInstantiation = false;
                return;
            }

            SpawnDestroyedVersion(transform);
        }
    
        protected void SpawnDestroyedVersion(Transform entity)
        {
            if (destroyedVersion != null)
            {
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(entity.position);
                bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (onScreen)
                {
                    GameObject destroyed = Instantiate(destroyedVersion);
                    destroyed.transform.position = entity.position;
                    destroyed.transform.eulerAngles = new Vector3(0f, entity.eulerAngles.y, 0f);
                    Destroy(destroyed, 3f);
                }
            }
        }
    }
}
