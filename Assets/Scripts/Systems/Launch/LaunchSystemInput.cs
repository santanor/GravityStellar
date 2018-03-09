using System;
using InputSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.UIElements;

namespace LaunchSystem
{
    public class LaunchSystemInput : MonoBehaviour
    {

        public delegate void LaunchProcessEvent( Vector2 screenPos, Vector3 worldPos );

        public delegate void LaunchEvent( Vector2 direction, float distanceFromOrigin );

        public InputManager InputManager;
        public LaunchProcessEvent OnLaunchProcessStart;
        public LaunchProcessEvent OnLaunchProcessDrag;
        public LaunchProcessEvent OnLaunchProcessFinish;

        void Awake()
        {
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);

            InputManager.OnLongTouch+= OnLongTouch;
            InputManager.OnDrag += OnDrag;
            InputManager.OnTouchFinish += OnTouchFinish;
        }

        void OnTouchFinish( Vector2 screenpos, Vector2 worldpos )
        {
            OnLaunchProcessFinish?.Invoke(screenpos, worldpos);
        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {
            OnLaunchProcessDrag?.Invoke(screenpos, worldpo);
        }

        void OnLongTouch( Vector2 screenpos, Vector2 worldpos )
        {
            OnLaunchProcessStart?.Invoke(screenpos, worldpos);
        }
    }
}
