using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using InputSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace SelectSystem
{
    public class SelectSystem : MonoBehaviour
    {
        [SerializeField] IList<Selectable> _selectables;

        StatusEnum _status;
        public Rect Rect;

        //Used to store the first drag position.
        public InputManager InputManager;

        public Vector3[] Corners = new Vector3[4];

        void Start()
        {
            _selectables = new List<Selectable>();
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);
            InputManager.OnDrag += OnDrag;
            InputManager.OnDragFinish += OnDragFinish;
            InputManager.OnTouch += OnTouch;
        }

        void OnDisable()
        {
            InputManager.OnDrag -= OnDrag;
            InputManager.OnDragFinish -= OnDragFinish;
            InputManager.OnTouch -= OnTouch;
        }

        /// <summary>
        ///     Called From each selectable to subscribe itself to the current system
        /// </summary>
        /// <param name="s"></param>
        public void AddSelectable( Selectable s )
        {
            if(_selectables == null) _selectables = new List<Selectable>();
            _selectables.Add(s);
        }

        /// <summary>
        ///     Called From each selectable to delete itself to the current system
        /// </summary>
        /// <param name="s"></param>
        public void RemoveSelectable( Selectable s )
        {
            _selectables.Remove(s);
        }

        void OnTouch( Vector2 screenpos, Vector2 worldpos )
        {
            RestartSelecteds();
            RestartBounds();
            Corners[0]= worldpos;//First corner
        }

        void OnDragFinish( Vector2 screenpos, Vector2 worldpo )
        {
            _status = StatusEnum.Idle;
        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {
            _status = StatusEnum.Selecting;
            CalculateBoundCorners(worldpo);
            Rect = new Rect
            {
                center = ( Corners[0] + Corners[3] ) / 2,
                yMax = Corners.Max(c => c.y),
                yMin = Corners.Min(c => c.y),
                xMax = Corners.Max(c => c.x),
                xMin = Corners.Min(c => c.x)
            };

            SelectWithinBounds();
        }

        /// <summary>
        /// For every object, check whether is within the bounds and change its state to selected (Or not!! :D )
        /// </summary>
        void SelectWithinBounds()
        {
            foreach (var selectable in _selectables)
            {
                if (Rect.Contains(selectable.transform.position, true))
                {
                    selectable.Select();
                }
                else
                {
                    selectable.Deselect();
                }
            }
        }

        /// <summary>
        ///     Restart all the selectables to 'idle'
        /// </summary>
        void RestartSelecteds()
        {
            foreach (var selectable in _selectables) selectable.Deselect();
        }

        void RestartBounds()
        {
            Rect = new Rect();
            for (var i = 0; i < Corners.Length; i++)
            {
                 Corners[i] = Vector2.zero;
            }
        }

        /// <summary>
        ///     Assigns the corners of the bounds creating a square from the first finger pos and the current one
        /// </summary>
        void CalculateBoundCorners( Vector2 currentFingerPos )
        {
            Corners[1] = new Vector2(Corners[0].x, currentFingerPos.y);
            Corners[2] = new Vector2(currentFingerPos.x, Corners[0].y);
            Corners[3] = currentFingerPos;
        }

        enum StatusEnum
        {
            Idle,
            Selecting
        }
    }

}
