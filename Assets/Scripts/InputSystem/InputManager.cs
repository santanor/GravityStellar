using System.Security.Cryptography;
using System.Security.Permissions;
using UnityEngine;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public delegate void TouchEvent( Vector2 screenPos, Vector2 worldPos );
        public delegate void DragEvent( Vector2 screenPos, Vector2 worldPo);

        public TouchEvent OnTouch;
        public DragEvent OnDrag;

        void Update()
        {
            var finger = Input.GetTouch(0);//Try get the first finger on the screen

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (finger.phase == TouchPhase.Ended)
            {
                OnTouch?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
            }

            if (finger.phase == TouchPhase.Moved)
            {
                OnDrag?.Invoke(finger.position, Camera.main.ScreenToWorldPoint(finger.position));
            }
        }
    }
}
