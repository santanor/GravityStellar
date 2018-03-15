using UnityEngine;
using UnityEngine.Assertions;

namespace Models
{
    public class DotToDotCollisionCallback : MonoBehaviour
    {
        public Dot Dot;
        public ParticleSystem ParticleSystem;

        void Awake()
        {
            Assert.IsNotNull(ParticleSystem);
            Assert.IsNotNull(Dot);


        }
    }
}
