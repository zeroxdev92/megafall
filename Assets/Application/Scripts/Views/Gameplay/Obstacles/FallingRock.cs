using UnityEngine;

namespace Application.Scripts.Views.Gameplay.Obstacles
{
    public class FallingRock : Obstacle {

        public GameObject rockMesh;

        void OnEnable()
        {
            rockMesh.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), 0f, 0f);
        }
    }
}
