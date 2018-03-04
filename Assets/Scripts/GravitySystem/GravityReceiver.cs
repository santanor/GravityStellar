using UnityEngine;

namespace GravitySystem
{
    public class GravityReceiver : MonoBehaviour
    {
        public GravitySystem GravitySystem;
        public Rigidbody2D Rigidbody2D;

        void Start()
        {
            GravitySystem.OnNewGravitySource += NewGravitySource;
            GravitySystem.OnDestroyedGravitySource += DestroyedGravitySource;

            foreach (var gs in GravitySystem.GravitySources) gs.OnGravityPulse += OnGravityPulse;
        }

        void OnDestroy()
        {
            GravitySystem.OnNewGravitySource -= NewGravitySource;
            GravitySystem.OnDestroyedGravitySource -= DestroyedGravitySource;

            foreach (var gs in GravitySystem.GravitySources) gs.OnGravityPulse -= OnGravityPulse;
        }

        void DestroyedGravitySource( GravitySource gs )
        {
            gs.OnGravityPulse += OnGravityPulse;
        }

        void NewGravitySource( GravitySource gs )
        {
            gs.OnGravityPulse -= OnGravityPulse;
        }


        /// <summary>
        ///     Applies the different gravity forces onto the current element
        /// </summary>
        /// <param name="sourcepoint"></param>
        /// <param name="pullforce"></param>
        void OnGravityPulse( Vector3 sourcepoint, float pullforce )
        {
            //get the offset between the objects
            var pullDirection = sourcepoint - transform.position;

            //we're doing 2d physics, so don't want to try and apply z forces!
            pullDirection.z = 0;

            //get the squared distance between the objects
            var magsqr = pullDirection.sqrMagnitude;

            //check distance is more than 0 (to avoid division by 0) and then apply a gravitational force to the object
            //note the force is applied as an acceleration, as acceleration created by gravity is independent of the mass of the object
            if (magsqr <= 0.6f)
            {
                OnDotTooClose();
            }
            else
            {
                var force = pullforce * pullDirection.normalized / magsqr;
                Rigidbody2D.AddForce(force, ForceMode2D.Force);
                var clampedVelocity = Mathf.Clamp(Rigidbody2D.velocity.magnitude, 0, 3);
                //Rigidbody2D.velocity = Rigidbody2D.velocity.normalized * clampedVelocity;
            }
        }


        protected virtual void OnDotTooClose()
        {
        }
    }
}