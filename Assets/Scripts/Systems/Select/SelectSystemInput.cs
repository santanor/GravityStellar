using InputSystem;
using Runtime_sets;
using UnityEngine;
using UnityEngine.Assertions;

namespace SelectSystem
{
    public class SelectSystemInput : MonoBehaviour
    {
        public delegate void SelectProcessEvent( Vector2 screenPos, Vector2 worldPos );

        public InputManager InputManager;
        public SelectedDotsSet SelectedDotsSet;
        public SelectSystem SelectSystem;

        //Events
        public SelectProcessEvent OnSelectStart;
        public SelectProcessEvent OnSelectingDrag;
        public SelectProcessEvent OnSelectStop;
        public SelectProcessEvent OnSelectShortTouch;

        void Awake()
        {
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);
            Assert.IsNotNull(SelectedDotsSet);
            Assert.IsNotNull(SelectSystem);
            InputManager.OnDrag += OnDrag;
            InputManager.OnTouchFinish += OnTouchFinish;
            InputManager.OnLongTouch += OnLongTouch;
            InputManager.OnShortTouch += OnShortTouch;
        }

        void OnDestroy()
        {
            InputManager.OnDrag -= OnDrag;
            InputManager.OnTouchFinish -= OnTouchFinish;
            InputManager.OnLongTouch -= OnLongTouch;
            InputManager.OnShortTouch -= OnShortTouch;
        }

        void OnShortTouch( Vector2 screenpos, Vector2 worldpos )
        {
            OnSelectShortTouch?.Invoke(screenpos, worldpos);
        }

        /// <summary>
        /// Only accept the touch if there are no items selected
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void OnLongTouch( Vector2 screenpos, Vector2 worldpos )
        {
            if (SelectedDotsSet.Items.Count == 0)
            {
                OnSelectStart?.Invoke(screenpos, worldpos);
            }
        }

        /// <summary>
        /// Finish him!!
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void OnTouchFinish( Vector2 screenpos, Vector2 worldpos )
        {
            OnSelectStop?.Invoke(screenpos, worldpos);
        }

        /// <summary>
        /// Only accept the Drag if the mode is "selecting"
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void OnDrag( Vector2 screenpos, Vector2 worldpos )
        {
            if (SelectSystem.Status == SelectSystem.StatusEnum.Selecting)
            {
                OnSelectingDrag?.Invoke(screenpos, worldpos);
            }
        }


    }
}
