using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Models;
using SelectSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace LaunchSystem
{
    public class LaunchSystem : MonoBehaviour
    {
        public LaunchSystemInput SystemInput;
        public SelectSystem.SelectSystem SelectSystem;

        [Tooltip("Max force to be applied")]
        public float MaxForce = 3;
        [Tooltip("Defines the max distance that matches with the max force. ")]
        public float MaxDistanceForForce = 3;

        Vector2 _initialLaunchPos;


        void Awake()
        {
            Assert.IsNotNull(SystemInput);
            SystemInput.OnLaunchProcessFinish += Launch;
            SystemInput.OnLaunchProcessStart -= LaunchProcessStart;
        }


        void OnDestroy()
        {
            SystemInput.OnLaunchProcessFinish -= Launch;
            SystemInput.OnLaunchProcessStart -= LaunchProcessStart;
        }

        /// <summary>
        /// After all the chitchat a launch event has finally happened.
        /// Apply force and direction to all the selected items
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void Launch( Vector2 screenpos, Vector2 worldpos )
        {
            //Get the force clamped to the max force;
            var dst = Mathf.Clamp(Vector3.Distance(worldpos, _initialLaunchPos), 0, MaxDistanceForForce);
            var force = Mathf.Clamp((MaxForce * dst)/MaxDistanceForForce , 0, MaxForce);//Get the force clammped
            var dir = (_initialLaunchPos - (Vector2)worldpos).normalized;

            //Find All the dots
            //Now for each selected selectable apply the force on the desired direction
            foreach (var s in SelectSystem.GetSelected())
            {
                s.Rigidbody2D.AddForce(dir * force);
                s.Selectable.Deselect();
            }
        }

        /// <summary>
        /// Saves the initial position so that we can calculate the distance and the force
        /// when the finger lifts up
        /// </summary>
        /// <param name="screenpos"></param>
        /// <param name="worldpos"></param>
        void LaunchProcessStart( Vector2 screenpos, Vector2 worldpos )
        {
            _initialLaunchPos = worldpos;
        }
    }
}
