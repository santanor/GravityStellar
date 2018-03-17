using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using GravitySystem;
using Models;
using Runtime_sets;
using SelectSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace LaunchSystem
{
    public class LaunchSystem : MonoBehaviour
    {
        //TODO refactor this to receive a new object (Launcheable?)
        public delegate void LaunchEvent( Dot s );

        public LaunchEvent OnSelectedDotLaunched;

        public LaunchSystemInput SystemInput;
        public SelectedDotsSet SelectedDotsSet;

        [Tooltip("Max force to be applied")]
        public float MaxForce = 3;
        [Tooltip("Defines the max distance that matches with the max force. ")]
        public float MaxDistanceForForce = 3;

        Vector2 initialLaunchPos;


        void Awake()
        {
            Assert.IsNotNull(SystemInput);
            SystemInput.OnLaunchProcessFinish += Launch;
            SystemInput.OnLaunchProcessStart += LaunchProcessStart;
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
            var dst = Mathf.Clamp(Vector3.Distance(worldpos, initialLaunchPos), 0, MaxDistanceForForce);
            var force = Mathf.Clamp((MaxForce * dst)/MaxDistanceForForce , 0, MaxForce);//Get the force clammped
            var dir = (initialLaunchPos - worldpos).normalized;

            //Iterate over the collection this way because calling "Deselect" will remove the item from it
            //This way we avoid running into issues if we use a foreach
            while(SelectedDotsSet.Items.Count > 0)
            {
                SelectedDotsSet.Items[0].Key.gameObject.GetComponent<GravityReceiver>().Rigidbody2D.AddForce(dir * force);
                SelectedDotsSet.Items[0].Value.Deselect();
                OnSelectedDotLaunched?.Invoke(SelectedDotsSet.Items[0].Key);
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
            initialLaunchPos = worldpos;
        }
    }
}
