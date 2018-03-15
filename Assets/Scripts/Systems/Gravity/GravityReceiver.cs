using System.Linq;
using UnityEngine;

namespace GravitySystem
{
    public class GravityReceiver : MonoBehaviour
    {
        public GravitySystem GravitySystem;
        public Rigidbody2D Rigidbody2D;
        [Tooltip("A list of tag names assigned to Gravity Sources to ignore")]
        public string[] IgnoreGravitiesTags;

        [Tooltip("What distance is too close form the source. it simulates a colision")]
        public float TooCloseDistance = 0.06f;

        void Start()
        {
            GravitySystem.AddGravityReceiver(this);
            GravitySystem.OnNewGravitySource += NewGravitySource;
            GravitySystem.OnDestroyedGravitySource += DestroyedGravitySource;

            foreach (var gs in GravitySystem.GravitySources)
            {
                //Only suscribe to the event if the gravitySource isn't ignored
                if(!IgnoreGravitiesTags.Contains(gs.tag))
                {
                    gs.OnGravityPulse += OnGravityPulse;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            GravitySystem.RemoveGravityReceiver(this);
            GravitySystem.OnNewGravitySource -= NewGravitySource;
            GravitySystem.OnDestroyedGravitySource -= DestroyedGravitySource;

            foreach (var gs in GravitySystem.GravitySources) gs.OnGravityPulse -= OnGravityPulse;
        }

        void DestroyedGravitySource( GravitySource gs )
        {
            gs.OnGravityPulse -= OnGravityPulse;
        }

        void NewGravitySource( GravitySource gs )
        {
            if (!IgnoreGravitiesTags.Contains(gs.tag))
            {
                gs.OnGravityPulse += OnGravityPulse;
            }
        }


        /// <summary>
        ///     Applies the different gravity forces onto the current element
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sourcepoint"></param>
        /// <param name="pullforce"></param>
        void OnGravityPulse( GravitySource source, Vector3 sourcepoint, float pullforce )
        {
            //If the object is further away, just ignore the pulse
            if (Vector2.Distance(transform.position, sourcepoint) > source.InfluenceAreaRadius) return;

            //get the offset between the objects
            var pullDirection = sourcepoint - transform.position;

            //we're doing 2d physics, so don't want to try and apply z forces!
            pullDirection.z = 0;

            //get the squared distance between the objects
            var magsqr = pullDirection.sqrMagnitude;

            //check distance is more than 0 (to avoid division by 0) and then apply a gravitational force to the object
            //note the force is applied as an acceleration, as acceleration created by gravity is independent of the mass of the object
            if (magsqr <= TooCloseDistance)
            {
                OnDotTooClose(source);
            }
            else
            {
                var force = pullforce * pullDirection.normalized / magsqr;
                Rigidbody2D.AddForce(force, ForceMode2D.Force);
                //var clampedVelocity = Mathf.Clamp(Rigidbody2D.velocity.magnitude, 0, 5);
                //Rigidbody2D.velocity = Rigidbody2D.velocity.normalized * clampedVelocity;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, TooCloseDistance);
        }


        protected virtual void OnDotTooClose( GravitySource source )
        {
        }
    }
}
