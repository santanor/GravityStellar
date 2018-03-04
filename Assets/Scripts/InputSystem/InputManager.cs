using System;
using UnityEngine;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public delegate void DragEvent( Vector2 screenPos, Vector2 worldPo );

        public delegate void TouchEvent( Vector2 screenPos, Vector2 worldPos );

        public DragEvent OnDrag;
        public DragEvent OnDragFinish;

        public TouchEvent OnTouch;

        void Update()
        {
            if (Input.touchCount == 0) return;
            var finger = Input.GetTouch(0); //Try get the first finger on the screen


            switch (finger.phase)
            {
                case TouchPhase.Began:
                    OnTouch?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
                    break;
                case TouchPhase.Moved:
                    OnDrag?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
                    break;
                case TouchPhase.Ended:
                    OnDragFinish?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
