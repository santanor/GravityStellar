﻿using System;
using System.Collections.Generic;
using System.Linq;
using InputSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace SelectSystem
{
    public class SelectSystem : MonoBehaviour
    {
        [SerializeField] public IList<Selectable> Selectables;

        StatusEnum _status;

        public Vector3[] Corners = new Vector3[4];

        //Used to store the first drag position.
        public InputManager InputManager;
        public Rect Rect;

        void Start()
        {
            Selectables = new List<Selectable>();
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);
            InputManager.OnDrag += OnDrag;
            InputManager.OnTouchFinish += OnTouchFinish;
            InputManager.OnLongTouch += OnLongTouch;
            InputManager.OnShortTouch += OnShortTouch;
        }

        void OnDisable()
        {
            InputManager.OnDrag -= OnDrag;
            InputManager.OnTouchFinish -= OnTouchFinish;
            InputManager.OnLongTouch -= OnLongTouch;
            InputManager.OnShortTouch -= OnShortTouch;
        }

        /// <summary>
        ///     Called From each selectable to subscribe itself to the current system
        /// </summary>
        /// <param name="s"></param>
        public void AddSelectable( Selectable s )
        {
            if (Selectables == null) Selectables = new List<Selectable>();
            Selectables.Add(s);
        }

        /// <summary>
        ///     Called From each selectable to delete itself to the current system
        /// </summary>
        /// <param name="s"></param>
        public void RemoveSelectable( Selectable s )
        {
            Selectables.Remove(s);
        }

        /// <summary>
        ///     Starts a new selection routine via a long touch
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void OnLongTouch( Vector2 screenpos, Vector2 worldpos )
        {
            if(!HasItemsSelected())
            {
                _status = StatusEnum.Selecting;
                RestartSelecteds();
                RestartBounds();
                Corners[0] = worldpos; //First corner
            }
        }

        /// <summary>
        ///     Finger up. Restart the selected status
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpo"></param>
        void OnTouchFinish( Vector2 screenpos, Vector2 worldpo )
        {
            _status = StatusEnum.Idle;
        }

        /// <summary>
        ///     The finger is being dragged. Update the bounds and selected "selectables"
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpo"></param>
        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {
            if (_status == StatusEnum.Selecting)
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
        }

        /// <summary>
        ///     A short touch has happened. Cancel the selection process
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpo"></param>
        void OnShortTouch( Vector2 screenpos, Vector2 worldpo )
        {
            RestartSelecteds();
            RestartBounds();
            _status = StatusEnum.Idle;
        }

        /// <summary>
        ///     For every object, check whether is within the bounds and change its state to selected (Or not!! :D )
        /// </summary>
        void SelectWithinBounds()
        {
            foreach (var selectable in Selectables)
                if (Rect.Contains(selectable.transform.position, true))
                    selectable.Select();
                else
                    selectable.Deselect();
        }

        /// <summary>
        /// Returns whether there are items selected
        /// </summary>
        /// <returns></returns>
        public bool HasItemsSelected()
        {
            for (var i = 0; i < Selectables.Count; i++)
            {
                if (Selectables[i].Status == Selectable.StatusEnum.Selected)
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Restart all the selectables to 'idle'
        /// </summary>
        void RestartSelecteds()
        {
            foreach (var selectable in Selectables) selectable.Deselect();
        }

        void RestartBounds()
        {
            Rect = new Rect();
            for (var i = 0; i < Corners.Length; i++) Corners[i] = Vector2.zero;
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
