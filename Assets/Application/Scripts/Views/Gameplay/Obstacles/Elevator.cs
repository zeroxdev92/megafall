using DG.Tweening;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay.Obstacles
{
    public class Elevator : Obstacle
    {
        private Tweener currentMov;
        public float duration = 2f;
        public bool restartPosOnDestroy = false;
        private Vector3 startPos;

        void Start()
        {
            startPos = transform.localPosition;    
        }

        // Use this for initialization
        void OnEnable()
        {

            currentMov = transform.DOMoveY(YToReturnToPool, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    base.ReturnToPool();
                });

        }
    
        protected override void OnDisable()
        {
            if (currentMov != null)
            {
                currentMov.Kill(false);
            }

            if (restartPosOnDestroy)
            {
                transform.localPosition = startPos;
            }

            if (firstInstantiation)
            {
                base.OnDisable();
                return;
            }
        
            SpawnDestroyedVersion(transform);
        }
    }
}
