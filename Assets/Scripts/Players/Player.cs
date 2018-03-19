using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models
{
    public class Player:MonoBehaviour
    {
        [Tooltip("Stars owned by the player")]
        public List<Star> Stars;
        public Dot DotPrefab;
        public Sprite StarSprite;


        protected void Awake()
        {
            var allStars = FindObjectsOfType<Star>();
            //It's linq, but it does it once only :)
            Stars = allStars.Where(x => x.Owner == this).ToList();

            foreach (var star in allStars)
            {
                star.OnStarOwnershipChanged += OwnershipChanged;
            }
        }

        /// <summary>
        /// Check if the ownership is transfered from or to the current player
        /// </summary>
        /// <param name="star"></param>
        /// <param name="oldowner"></param>
        /// <param name="newowner"></param>
        void OwnershipChanged( Star star, Player oldowner, Player newowner )
        {
            //This player just lost this star
            if (oldowner == this)
            {
                Stars.Remove(star);
            }

            //This player just won this star!! :D
            if (newowner == this)
            {
                TransferStar(star);
                Stars.Add(star);

            }
        }

        /// <summary>
        /// Applies and changes all the necesary bits to transfer ownership of the star
        /// 1 - Deletes any dot parented to the star
        /// 2 - Changes the dot prefab
        /// 3 - Changes the visuals
        /// </summary>
        /// <param name="star"></param>
        void TransferStar(Star star)
        {
            //1 - Deletes any dot parented to the star
            foreach (var dot in star.Dots)
            {
                if(dot.gameObject != null)
                {
                    Destroy(dot.gameObject);
                }
            }

            //2 - Changes the dot prefab
            star.DotPrefab = DotPrefab;

            //3 - Changes the visuals
            star.ChangeSprite(StarSprite);

        }
    }
}
