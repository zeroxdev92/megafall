using UnityEngine;

namespace Application.Scripts.Views.Gameplay.Obstacles
{
    public class Destroyable : MonoBehaviour
    {
        public GameObject destroyedPrefab;

        void OnDisable()
        {
            if (destroyedPrefab != null)
            {
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
                bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (onScreen)
                {
                    GameObject destroyed = Instantiate(destroyedPrefab);
                    destroyed.transform.position = transform.position;
                    destroyed.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                    Destroy(destroyed, 3f);
                }
            }
        }
    }
}
