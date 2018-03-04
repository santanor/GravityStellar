using System;
using System.Collections;
using GravitySystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    public class Dot : GravityReceiver
    {
        public Star ParentStar;
        public float InitialOrbitVertialForce;
        public float InitialOrbitDeltaForce;
        public float InitialOrbitKickDuration = 1f;

        Vector2 _vDir;
        Vector2 _dDir;

        protected override void OnDotTooClose()
        {
            base.OnDotTooClose();
        }

        /// <summary>
        /// Gives the dot the initial kick to put it in orbit of the parent star
        /// </summary>
        public void InitialOrbitKick()
        {
            _vDir = Vector2.up;//(Random.insideUnitCircle - (Vector2)ParentStar.transform.position).normalized;
            _dDir = Vector2.right;//((Vector2)Vector3.Cross(Vector3.forward, vDir)).normalized;
            StartCoroutine(OrbitalKickRoutine());
        }

        /// <summary>
        /// Performs the initial force to put the Dot in orbit
        /// </summary>
        /// <returns></returns>
        IEnumerator OrbitalKickRoutine()
        {
            var ticker = 0f;

            while (ticker < InitialOrbitKickDuration)
            {
                transform.LookAt(ParentStar.transform.position);

                //Get the new up direction vector
                _vDir = - -transform.forward;
                _dDir = ((Vector2)Vector3.Cross(Vector3.forward, _vDir)).normalized;

                //Lerps the forces giving more force towards the vertical direction at the begining
                //And more horizontal force at the end
                var vLerped = Mathf.Lerp(InitialOrbitVertialForce, 0, ticker / InitialOrbitKickDuration);
                var dLerped = Mathf.Lerp(0, InitialOrbitDeltaForce, ticker / InitialOrbitKickDuration);
                Rigidbody2D.AddForce(_vDir*vLerped);
                Rigidbody2D.AddForce(_dDir* dLerped);

                ticker += Time.deltaTime;
                yield return null;
            }
        }
    }
}
