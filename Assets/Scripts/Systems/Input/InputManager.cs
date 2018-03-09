using System;
using UnityEngine;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public delegate void TouchEvent( Vector2 screenPos, Vector2 worldPos );
#if UNITY_EDITOR
        bool _isDragging;
#endif
        //Saves where the finger was at the begining of a short touch
        Vector2 _shortTouchInitPos;

        //Used to count the time between touch down and up
        float _ticker;

        public TouchEvent OnDrag;
        public TouchEvent OnLongTouch;
        public TouchEvent OnShortTouch;
        public TouchEvent OnTouchFinish;

        [Tooltip("Max time for the finger to stay down for it to be a short touch")]
        public float ShortTouchThreshold = 0.2f;


        void Update()
        {
#if UNITY_EDITOR
            EditorInput();
#else
            MobileInput();
            #endif
        }


        /// <summary>
        ///     Input system used when the app is running in the editor. Mainly debugger
        /// </summary>
        void EditorInput()
        {
            //Starts a short touch
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Input.GetMouseButton(0) && _shortTouchInitPos == Vector2.zero)
            {
                _ticker = 0;
                _shortTouchInitPos = Input.mousePosition;
            }

            //After the short touch threshold, continue as
            if (Input.GetMouseButton(0) && _ticker > ShortTouchThreshold && !_isDragging)
            {
                _isDragging = true;
                OnLongTouch?.Invoke(_shortTouchInitPos, Camera.main.ScreenToWorldPoint(_shortTouchInitPos));
            }

            //The finger is being dragged
            if (Input.GetMouseButton(0) && _isDragging)
                OnDrag?.Invoke(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Finger up
            if (Input.GetMouseButtonUp(0))
            {
                //A Short touch
                if (_ticker < ShortTouchThreshold)
                    OnShortTouch?.Invoke(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                else //A Long touch
                    OnTouchFinish?.Invoke(Input.mousePosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));

                //Restart the variables on finger up
                _ticker = 0;
                _isDragging = false;
                _shortTouchInitPos = Vector2.zero;
            }

            //Increment the ticker
            _ticker += Time.deltaTime;
        }

        //TODO Update input method to match the one in the editor
        /// <summary>
        ///     Input system used when the app is running on a mobile device
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void MobileInput()
        {
            if (Input.touchCount == 0) return;
            var finger = Input.GetTouch(0); //Try get the first finger on the screen


            switch (finger.phase)
            {
                case TouchPhase.Began:
                    OnLongTouch?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
                    break;
                case TouchPhase.Moved:
                    OnDrag?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
                    break;
                case TouchPhase.Ended:
                    OnTouchFinish?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
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
