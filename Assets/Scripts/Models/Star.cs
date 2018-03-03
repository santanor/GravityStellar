using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Star : GravitySource
    {
        public GravitySystem.GravitySystem GravitySystem;
        public float MinSpawnRate;
        public float MaxSpawnRate;
        public Dot DotPrefab;
        public IList<Dot> Dots;


        float _spawnTicker = 0f;

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
        /// Uses the spawnRate to create a new Dot when it's time
        /// </summary>
        void SpawnDotRoutine()
        {
            //When the ticker gets to 0, Spawn the Dot and restart it 
            if (_spawnTicker <= 0)
            {
                _spawnTicker  = Random.Range(MinSpawnRate, MaxSpawnRate);
                var d = Instantiate(DotPrefab, transform);
                d.ParentStar = this;
                Dots.Add(d);
            }

            _spawnTicker -= Time.deltaTime;
        }
    }
}
