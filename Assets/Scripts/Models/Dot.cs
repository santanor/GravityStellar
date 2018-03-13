using System.Collections;
using GravitySystem;
using SelectSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Models
{
    public class Dot : GravityReceiver
    {
        Vector2 dDir;
        Vector2 vDir;

        [Tooltip("Horizontal force magnitude")]
        public float InitialOrbitDeltaForce;

        [Tooltip("Duration of the orbital kick")]
        public float InitialOrbitKickDuration = 1f;

        [Tooltip("Vertical force magnitude")] public float InitialOrbitVertialForce;

        public Star ParentStar;

        protected override void OnDotTooClose()
        {
            base.OnDotTooClose();
        }

        /// <summary>
        ///     Gives the dot the initial kick to put it in orbit of the parent star
        /// </summary>
        public void InitialOrbitKick()
        {
            vDir = ( Random.insideUnitCircle - (Vector2) ParentStar.transform.position ).normalized;
            dDir = ( (Vector2) Vector3.Cross(Vector3.forward, vDir) ).normalized;

            //Perform an initial kick to move it from (0,0,0)
            Rigidbody2D.AddForce(vDir);
            Rigidbody2D.AddForce(dDir);

            StartCoroutine(OrbitalKickRoutine());
        }

        /// <summary>
        ///     Performs the initial force to put the Dot in orbit
        /// </summary>
        /// <returns></returns>
        IEnumerator OrbitalKickRoutine()
        {
            var ticker = 0f;

            while (ticker < InitialOrbitKickDuration)
            {
                transform.LookAt(ParentStar.transform.position);

                //Get the new up direction vector
                vDir = -transform.forward;
                dDir = ( (Vector2) Vector3.Cross(Vector3.forward, vDir) ).normalized;

                //Lerps the forces giving more force towards the vertical direction at the begining
                //And more horizontal force at the end
                var vLerped = Mathf.Lerp(InitialOrbitVertialForce, 0, ticker / InitialOrbitKickDuration);
                var dLerped = Mathf.Lerp(0, InitialOrbitDeltaForce, ticker / InitialOrbitKickDuration);
                Rigidbody2D.AddForce(vDir * vLerped);
                Rigidbody2D.AddForce(dDir * dLerped);

                ticker += Time.deltaTime;
                yield return null;
            }
        }
    }
}
