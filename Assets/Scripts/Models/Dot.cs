using System;
using System.Collections;
using GravitySystem;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Models
{
    public class Dot : GravityReceiver
    {
        public delegate void DotDestroyedEvent( Dot dot, Vector2 deadVelocity, bool deadByColision );

        Vector2 dDir;

        /// <summary>
        ///     The velocity when the dot died. It's used as cache so that when the simulation stops
        ///     we still know how fast it was going
        /// </summary>
        Vector2 dyingVelocity;

        [Tooltip("Horizontal force magnitude")]
        public float InitialOrbitDeltaForce;

        [Tooltip("Duration of the orbital kick")]
        public float InitialOrbitKickDuration = 1f;

        [Tooltip("Vertical force magnitude")] public float InitialOrbitVertialForce;

        [Tooltip("Time (In seconds) to destroy a Dot after its been launched")]
        public float LaunchedDotDestroyDelayMin = 7f;
        public float LaunchedDotDestroyDelayMax = 10f;

        /// <summary>
        ///     Only true when a dot is being destroyed
        /// </summary>
        bool isBeingDestroyed;

        bool isDestroyedForColision;

        public LaunchSystem.LaunchSystem LaunchSystem;
        public DotDestroyedEvent OnDotDestroyed;

        public Star ParentStar;
        Vector2 vDir;

        void Awake()
        {
            LaunchSystem = FindObjectOfType<LaunchSystem.LaunchSystem>();
            Assert.IsNotNull(LaunchSystem);
            LaunchSystem.OnSelectedDotLaunched += DotLaunched;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDotDestroyed?.Invoke(this, dyingVelocity, isDestroyedForColision);
            LaunchSystem.OnSelectedDotLaunched -= DotLaunched;
        }

        /// <summary>
        ///     This dot has been launched
        ///     Start a death timer, which will kill the dot unless it colides first
        /// </summary>
        /// <param name="s"></param>
        void DotLaunched( Dot s )
        {
            //Of course, only do this if the dot launched is the current one
            if (s == this)
            {
                ParentStar.OnGravityPulse -= OnGravityPulse;
                StartCoroutine(DestroyDotRoutine());
            }
        }

        /// <summary>
        /// After the delay LaunchedDotDestroyDelay destroy the object
        /// ONLY! if it hasn't been destroyed already
        /// </summary>
        /// <returns></returns>
        IEnumerator DestroyDotRoutine()
        {
            yield return new WaitForSeconds(Random.Range(LaunchedDotDestroyDelayMin, LaunchedDotDestroyDelayMax));
            if (!isBeingDestroyed)//Only destroy if it;s not being destroyed already
            {
                dyingVelocity = Rigidbody2D.velocity;
                isBeingDestroyed = true;
                transform.forward = Rigidbody2D.velocity.normalized;
                Destroy(gameObject);
            }
        }

        /// <summary>
        ///     When the dot is too close to a gravitySource
        /// </summary>
        /// <param name="source"></param>
        protected override void OnDotTooClose( GravitySource source )
        {
            base.OnDotTooClose(source);

            //If the source is a Dot then we display the colision particles and destroy the Dot.
            if (source.GetComponent<Dot>() && !isBeingDestroyed)
            {
                //Routine to destroy a dot
                ParentStar.Dots.Remove(this);
                dyingVelocity = Rigidbody2D.velocity;
                isBeingDestroyed = true;
                transform.forward = Rigidbody2D.velocity.normalized;
                isDestroyedForColision = true;
                Destroy(gameObject, 0.01f); //Give it time to process the other dot
            }

            //If the source is a Star (And it's not a star owned by the player that owns the parent)
            //Then destroy the dot and take damage from the star
            var star = source.GetComponent<Star>();
            if (star != null && !isBeingDestroyed && ParentStar.Owner != star.Owner)
            {
                isBeingDestroyed = true;
                ParentStar.Dots.Remove(this);
                star.TakeDamage(this);
                Destroy(gameObject);
            }
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
