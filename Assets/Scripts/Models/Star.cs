using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [RequireComponent(typeof(GravitySource))]
    public class Star : MonoBehaviour
    {
        public GravitySystem.GravitySystem GravitySystem;
        public float MinSpawnRate;
        public float MaxSpawnRate;
        public GravitySource GravitySource;
        public Dot DotPrefab;
        public IList<Dot> Dots;


    }
}
