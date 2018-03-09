using UnityEngine;

namespace LaunchSystem
{
    public class LaunchSystemGraphicFeedback : MonoBehaviour
    {
        Vector2 _initialLaunchPos;
        SpriteRenderer _sprite;
        public GameObject Arrow;
        public LaunchSystem LaunchSystem;
        public LaunchSystemInput SystemInput;

        void Awake()
        {
            _sprite = Arrow.GetComponentInChildren<SpriteRenderer>();
            SystemInput.OnLaunchProcessStart += BeginShowingArrow;
            SystemInput.OnLaunchProcessDrag += UpdateArrow;
            SystemInput.OnLaunchProcessFinish += StopShowingArrow;
        }

        void OnDestroy()
        {
            SystemInput.OnLaunchProcessStart -= BeginShowingArrow;
            SystemInput.OnLaunchProcessDrag -= UpdateArrow;
            SystemInput.OnLaunchProcessFinish -= StopShowingArrow;
        }

        /// <summary>
        ///     Make the arrow look in the direction created from the initial pos and the current finger pos
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void UpdateArrow( Vector2 screenpos, Vector3 worldpos )
        {
            //By doing this we can then only extrude the sprite in the "width"
            Arrow.transform.LookAt(_initialLaunchPos);

            var dst = Vector2.Distance(worldpos, _initialLaunchPos);
            _sprite.size = new Vector2(dst, _sprite.size.y);
        }

        /// <summary>
        ///     Simply hides the arrow when the finger lifts up
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void StopShowingArrow( Vector2 screenpos, Vector3 worldpos )
        {
            Arrow.SetActive(false);
        }

        /// <summary>
        ///     Shows the arrow and saves the initial pos
        ///     so that we know from where to where to draw the arrow
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void BeginShowingArrow( Vector2 screenpos, Vector3 worldpos )
        {
            _initialLaunchPos = worldpos;
            _sprite.size = Vector2.zero;
            Arrow.SetActive(true);
        }
    }
}
