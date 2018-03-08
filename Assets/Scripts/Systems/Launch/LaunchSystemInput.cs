using System;
using InputSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.UIElements;

namespace LaunchSystem
{
    public class LaunchSystemInput : MonoBehaviour
    {

        public delegate void LaunchEvent( Vector2 cameraPos, Vector3 worldPos );

        [Tooltip("Delta between tap and release to make a 'touch' a short one")]
        public float ShortTouchThreshold = 0.2f;

        public InputManager InputManager;
        public LaunchEvent OnLaunchStart;
        public LaunchEvent OnLaunchDrag;
        public LaunchEvent OnLaunchFinish;

        /// <summary>
        /// Used to check if a touch is a short one.
        /// </summary>
        float _ticker;

        void Awake()
        {
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);

            InputManager.OnLongTouch+= OnTouch;
            InputManager.OnDrag += OnDrag;
            InputManager.OnTouchFinish += OnTouchFinish;
        }

        void OnTouchFinish( Vector2 screenpos, Vector2 worldpos )
        {

        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {

        }

        void OnTouch( Vector2 screenpos, Vector2 worldpos )
        {
            //_ticker = DateTime.Now.;
        }
    }
}
