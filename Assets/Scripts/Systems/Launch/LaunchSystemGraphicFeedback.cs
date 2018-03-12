using UnityEditor;
using UnityEngine;

namespace LaunchSystem
{
    public class LaunchSystemGraphicFeedback : MonoBehaviour
    {
        Vector2 _initialLaunchPos;
        LineRenderer _lineRenderer;
        public GameObject Arrow;
        public LaunchSystem LaunchSystem;
        public LaunchSystemInput SystemInput;

        void Awake()
        {
            _lineRenderer = Arrow.GetComponentInChildren<LineRenderer>();
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
        ///  Make the arrow look in the direction created from the initial pos and the current finger pos
        ///  but clampped to the max distance defined in the LaunchSystem
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void UpdateArrow( Vector2 screenpos, Vector3 worldpos )
        {
            Arrow.transform.LookAt(_initialLaunchPos);
            Arrow.transform.position = worldpos;

            //It means there will be no more force even if the finger is even more further away
            //So what we do is clamp the arrow to be the max size
            if (Vector3.Distance((Vector3) _initialLaunchPos, worldpos) > LaunchSystem.MaxDistanceForForce)
            {
                //To do that we find the direction from the finger to the initial pos
                //Add that direction to the initial position
                //Multiply that by the max distance
                var dir = ( _initialLaunchPos - (Vector2) worldpos ).normalized;
                var pos = _initialLaunchPos - dir * LaunchSystem.MaxDistanceForForce;
                _lineRenderer.SetPositions(new Vector3[] {pos, _initialLaunchPos});
            }
            else
            {
                _lineRenderer.SetPositions(new [] {worldpos, (Vector3)_initialLaunchPos});
            }
            _lineRenderer.positionCount = 2;
        }

        /// <summary>
        ///  Simply hides the arrow when the finger lifts up
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
            Arrow.SetActive(true);
        }
    }
}
