using UnityEngine;
using UnityEngine.Assertions;

namespace Models
{
    public class DotDestroyedParticles : MonoBehaviour
    {
        public Dot Dot;
        public ParticleSystem ParticleSystem;

        void Awake()
        {
            Assert.IsNotNull(Dot);
            Assert.IsNotNull(ParticleSystem);

            Dot.OnDotDestroyed += DotDestroyed;
        }

        /// <summary>
        /// Called when a dot is destroyed, it'll burst the particles
        /// </summary>
        void DotDestroyed(Dot dot, Rigidbody2D d )
        {
            //Get the particle object and unparent it from the Dot so that we can destroy it and the
            //Particles keep on executing
            var main = ParticleSystem.main;
            //main.startSpeed = d.velocity.magnitude;
            ParticleSystem.gameObject.transform.parent = null;
            ParticleSystem.Play();

            //Destroy in a bit to give it time to play the particles
            Destroy(ParticleSystem.gameObject, 0.5f);

        }
    }
}
