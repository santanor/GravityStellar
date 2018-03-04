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
            SelectSystem = ScriptableObject.CreateInstance<SelectSystem>();
            Manager.OnDrag += OnDrag;
            Manager.OnDragFinish += OnDragFinish;
            Manager.OnTouch += OnTouch;

            LineRenderer.enabled = false;
        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {
            LineRenderer.enabled = true;
            //Get the 4 corners from the Select System and assign them to the line renderer
            LineRenderer.SetPositions(new Vector3[]
            {
                SelectSystem.FirstPos, SelectSystem.SecondPos, SelectSystem.FourthPos,
                 SelectSystem.ThirdPos
            });

            LineRenderer.positionCount = 4;
        }

        void OnDragFinish( Vector2 screenpos, Vector2 worldpo )
        {
            LineRenderer.enabled = false;
        }

        void OnTouch( Vector2 screenpos, Vector2 worldpos )
        {
            LineRenderer.enabled = false;
        }
    }
}
