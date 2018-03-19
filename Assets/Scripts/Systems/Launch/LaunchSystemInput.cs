using System.Linq;
using InputSystem;
using Runtime_sets;
using SelectSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace LaunchSystem
{
    public class LaunchSystemInput : MonoBehaviour
    {
        public delegate void LaunchEvent( Vector2 direction, float distanceFromOrigin );

        public delegate void LaunchProcessEvent( Vector2 screenPos, Vector2 worldPos );

        public InputManager InputManager;
        public SelectedDotsSet SelectedDotsSet;
        public SelectSystem.SelectSystem SelectSystem;

        //Events
        public LaunchProcessEvent OnLaunchProcessDrag;
        public LaunchProcessEvent OnLaunchProcessFinish;
        public LaunchProcessEvent OnLaunchProcessStart;

        void Awake()
        {
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);
            Assert.IsNotNull(SelectedDotsSet);
            InputManager.OnLongTouch += OnLongTouch;
            InputManager.OnDrag += OnDrag;
            InputManager.OnTouchFinish += OnTouchFinish;
        }

        void OnTouchFinish( Vector2 screenpos, Vector2 worldpos )
        {
            if (SelectedDotsSet.Items.Count > 0
                && SelectSystem.Status == global::SelectSystem.SelectSystem.StatusEnum.Idle)
            {
                OnLaunchProcessFinish?.Invoke(screenpos, worldpos);
            }
        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {
            if (SelectedDotsSet.Items.Count > 0
                && SelectSystem.Status == global::SelectSystem.SelectSystem.StatusEnum.Idle)
            {
                OnLaunchProcessDrag?.Invoke(screenpos, worldpo);
            }

        }

        void OnLongTouch( Vector2 screenpos, Vector2 worldpos )
        {
            //Only start this mode if there are items selected
            if (SelectedDotsSet.Items.Count > 0)
            {
                OnLaunchProcessStart?.Invoke(screenpos, worldpos);
            }

        }
    }
}
