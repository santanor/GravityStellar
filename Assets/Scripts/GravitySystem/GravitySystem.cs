using System.Collections.Generic;
using UnityEngine;

namespace GravitySystem
{
    public class GravitySystem : ScriptableObject
    {
        public delegate void NewGravitySourceEvent( GravitySource gs );
        public delegate void DestroyedGravitySourceEvent( GravitySource gs );

        public NewGravitySourceEvent OnNewGravitySource;
        public DestroyedGravitySourceEvent OnDestroyedGravitySource;

        public IList<GravitySource> GravitySources;


        void Awake()
        {
            GravitySources = new List<GravitySource>();
        }

        void AddGravitySource( GravitySource gs )
        {

        }


    }
}
