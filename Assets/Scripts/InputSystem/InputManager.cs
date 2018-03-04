using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public delegate void DragEvent( Vector2 screenPos, Vector2 worldPo );

        public delegate void TouchEvent( Vector2 screenPos, Vector2 worldPos );

        public DragEvent OnDrag;
        public DragEvent OnDragFinish;

        public TouchEvent OnTouch;

        #if UNITY_EDITOR
        bool _isDragging;
        #endif


        void Update()
        {
            #if UNITY_EDITOR
            EditorInput();
#else
            MobileInput();
            #endif
        }


        /// <summary>
        /// Input system used when the app is running in the editor. Mainly debugger
        /// </summary>
        void EditorInput()
        {
            if (Input.GetMouseButton(0) && !_isDragging)
            {
                _isDragging = true;
                OnTouch?.Invoke(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            if (Input.GetMouseButton(0) && _isDragging)
            {
                OnDrag?.Invoke(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                OnDragFinish?.Invoke(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        /// <summary>
        /// Input system used when the app is running on a mobile device
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void MobileInput()
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
