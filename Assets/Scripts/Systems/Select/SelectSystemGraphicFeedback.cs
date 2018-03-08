using System.Linq;
using InputSystem;
using UnityEngine;

namespace SelectSystem
{
    public class SelectSystemGraphicFeedback : MonoBehaviour
    {
        public LineRenderer LineRenderer;
        public InputManager Manager;
        public SelectSystem SelectSystem;

        void Awake()
        {
            Manager.OnDrag += OnDrag;
            Manager.OnTouchFinish += OnTouchFinish;
            Manager.OnLongTouch+= OnLongTouch;

            LineRenderer.enabled = false;
        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {
            LineRenderer.enabled = true;
            //Get the 4 corners from the Select System and assign them to the line renderer
            // ReSharper disable once SuspiciousTypeConversion.Global
            LineRenderer.SetPositions(new Vector3[]
            {
                SelectSystem.Corners[0], SelectSystem.Corners[1], SelectSystem.Corners[3], SelectSystem.Corners[2]
            });

            LineRenderer.positionCount = 4;
        }

        void OnTouchFinish( Vector2 screenpos, Vector2 worldpo )
        {
            LineRenderer.enabled = false;
        }

        void OnLongTouch( Vector2 screenpos, Vector2 worldpos )
        {
            LineRenderer.enabled = false;
        }
    }
}
