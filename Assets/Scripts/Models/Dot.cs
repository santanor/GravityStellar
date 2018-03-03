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

        Vector2 vDir;
        Vector2 dDir;

        protected override void OnDotTooClose()
        {
            base.OnDotTooClose();
        }

        /// <summary>
        /// Gives the dot the initial kick to put it in orbit of the parent star
        /// </summary>
        public void InitialOrbitKick()
        {
            vDir = Vector2.up;//(Random.insideUnitCircle - (Vector2)ParentStar.transform.position).normalized;
            dDir = Vector2.right;//((Vector2)Vector3.Cross(Vector3.forward, vDir)).normalized;
            StartCoroutine(OrbitalKickRoutine());
        }

        IEnumerator OrbitalKickRoutine()
        {
            var ticker = 0f;

            while (ticker < InitialOrbitKickDuration)
            {
                transform.LookAt(ParentStar.transform.position);

                //Get the new up direction vector
                vDir = - -transform.forward;
                dDir = ((Vector2)Vector3.Cross(Vector3.forward, vDir)).normalized;
                var vLerped = Mathf.Lerp(InitialOrbitVertialForce, 0, ticker / InitialOrbitKickDuration);
                var dLerped = Mathf.Lerp(0, InitialOrbitDeltaForce, ticker / InitialOrbitKickDuration);
                Rigidbody2D.AddForce(vDir*vLerped);
                Rigidbody2D.AddForce(dDir* dLerped);

                ticker += Time.deltaTime;
                yield return null;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, vDir);
            Gizmos.DrawRay(transform.position, dDir);
        }
    }
}
