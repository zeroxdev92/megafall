using System;
using System.Collections.Generic;
using Application.Scripts.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Application.Scripts.Views.Gameplay
{
    public class TouchController : MonoBehaviour
    {

        public Action<SwipeDirection> OnSwipe;

        public float maxSwipeTime; 
        public float minSwipeDistance;

        private float startTime;
        private float endTime;

        private Vector3 startPos;
        private Vector3 endPos;
        private float swipeDistance;
        private float swipeTime;
        private bool swiped = false;
        private bool inputMoved = false;

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                CheckInput(touch);
            }
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void CheckInput(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;
                inputMoved = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                CheckSwipe(touch);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (!inputMoved)
                {
                    CheckSwipe(touch);
                }

                swiped = false;
            }
        }

        private void CheckSwipe(Touch touch)
        {
            if (!swiped)
            {
                endTime = Time.time;
                endPos = touch.position;

                Vector2 dir = endPos - startPos;

                swipeDistance = dir.sqrMagnitude;

                swipeTime = endTime - startTime;
            
                if (swipeTime < maxSwipeTime && swipeDistance >= minSwipeDistance)
                {
                    Swipe(dir);
                    inputMoved = true;
                }
            }
        }

        private void Swipe(Vector2 dir)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    OnSwipe?.Invoke(SwipeDirection.Right);
                }
                else if (dir.x < 0)
                {
                    OnSwipe?.Invoke(SwipeDirection.Left);
                }
            }
            else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
            {
                if (dir.y > 0)
                {
                    OnSwipe?.Invoke(SwipeDirection.Up);
                }
                else if (dir.y < 0)
                {
                    OnSwipe?.Invoke(SwipeDirection.Down);
                }
            }

            swiped = true;
        }
    }
}
