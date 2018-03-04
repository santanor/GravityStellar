using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Star : GravitySource
    {
        float _spawnTicker;
        public Dot DotPrefab;
        public IList<Dot> Dots;
        public GravitySystem.GravitySystem GravitySystem;
        public float MaxSpawnRate;
        public float MinSpawnRate;

        void Start()
        {
            Dots = new List<Dot>();
            GravitySystem.AddGravitySource(this);
        }

        void OnDestroy()
        {
            GravitySystem.DestroyGravitySource(this);
        }

        void Update()
        {
            SpawnDotRoutine();
        }


        /// <summary>
        ///     Uses the spawnRate to create a new Dot when it's time
        /// </summary>
        void SpawnDotRoutine()
        {
            //When the ticker gets to 0, Spawn the Dot and restart it
            if (_spawnTicker <= 0)
            {
                _spawnTicker = Random.Range(MinSpawnRate, MaxSpawnRate);
                var d = Instantiate(DotPrefab, transform);
                d.ParentStar = this;
                d.InitialOrbitKick();
                Dots.Add(d);
            }

            _spawnTicker -= Time.deltaTime;
        }
    }
}