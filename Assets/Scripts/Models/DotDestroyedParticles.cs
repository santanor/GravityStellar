using UnityEngine;
using UnityEngine.Assertions;

namespace Models
{
    public class DotDestroyedParticles : MonoBehaviour
    {
        public Dot Dot;
        public ParticleSystem CollisionDeadParticles;
        public ParticleSystem TimeoutDeadParticles;

        void Awake()
        {
            Assert.IsNotNull(Dot);
            Assert.IsNotNull(CollisionDeadParticles);

            Dot.OnDotDestroyed += DotDestroyed;
        }

        /// <summary>s
        /// Called when a dot is destroyed, it'll burst the particles
        /// </summary>
        void DotDestroyed(Dot dot, Vector2 velocity, bool deadByColision )
        {
            gameObject.transform.SetParent(null);
            PlayParticleEffect(deadByColision ? CollisionDeadParticles : TimeoutDeadParticles, velocity);

            //Destroy in a bit to give it time to play the particles
            Destroy(gameObject, 2f);
        }

        /// <summary>
        /// Plays the paticles for the chosen death (that is collision or timeout)
        /// </summary>
        /// <param name="system"></param>
        /// <param name="velocity"></param>
        void PlayParticleEffect( ParticleSystem particles, Vector2 velocity)
        {
            //Get the particle object and unparent it from the Dot so that we can destroy it and the
            //Particles keep on executing
            var main = particles.main;
            main.startSpeed = velocity.magnitude;
            particles.Play();
        }
    }
}
