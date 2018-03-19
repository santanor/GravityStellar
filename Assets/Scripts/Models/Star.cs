using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Star : GravitySource
    {
        public delegate void StarDamagedEvent( Star star, float oldHealth, float newHealth );

        public delegate void StarOwnerEvent( Star star, Player oldOwner, Player newOwner );

        float spawnTicker;
        [Tooltip("Health points to destroy the star (Number of dots required)")]
        public int Health = 20;
        float currentHealth;
        [Tooltip("Health regen speed. Amount in seconds")]
        public float RegenSpeed = 0.1f;
        public Dot DotPrefab;
        public List<Dot> Dots;
        public float MaxSpawnRate;
        public float MinSpawnRate;

        [Tooltip("Current owner (Player) of the star")]
        public Player Owner;

        public SpriteRenderer SpriteRenderer;

        public StarDamagedEvent OnStarDamaged;
        public StarOwnerEvent OnStarOwnershipChanged;

        void Start()
        {
            Dots = new List<Dot>();
        }

        void Update()
        {
            SpawnDotRoutine();
            RegenHealth();
        }

        /// <summary>
        /// Regens the health (Unless at max health already)
        /// </summary>
        void RegenHealth()
        {
            var healthToRegen = RegenSpeed / Time.deltaTime;
            if (currentHealth + healthToRegen < Health)
            {
                currentHealth += healthToRegen;
            }
        }

        /// <summary>
        /// When a dot is too close the star takes damage.
        /// If the damage drops below zero it'll transfer ownership
        /// to the player that launched the dot
        /// </summary>
        /// <param name="dot"></param>
        public void TakeDamage( Dot dot )
        {
            //Each dot only makes 1 dmg
            currentHealth -= 1;
            OnStarDamaged?.Invoke(this, currentHealth+1, currentHealth);//Notify!!

            if (currentHealth < 0)//Star died
            {
                OnStarOwnershipChanged(this, Owner, dot.ParentStar.Owner);
            }
        }


        /// <summary>
        ///     Uses the spawnRate to create a new Dot when it's time
        /// </summary>
        void SpawnDotRoutine()
        {
            //When the ticker gets to 0, Spawn the Dot and restart it
            if (spawnTicker <= 0)
            {
                spawnTicker = Random.Range(MinSpawnRate, MaxSpawnRate);
                var d = Instantiate(DotPrefab, transform);
                d.ParentStar = this;
                d.InitialOrbitKick();
                Dots.Add(d);
            }

            spawnTicker -= Time.deltaTime;
        }

        /// <summary>
        /// Changes the visual of the star
        /// </summary>
        /// <param name="sprite"></param>
        public void ChangeSprite(Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;
        }
    }
}
