using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GravitySystem
{
    public class GravitySystem : ScriptableObject
    {
        public delegate void DestroyedGravitySourceEvent( GravitySource gs );

        public delegate void NewGravitySourceEvent( GravitySource gs );

        public IList<GravitySource> GravitySources;
        public DestroyedGravitySourceEvent OnDestroyedGravitySource;

        public NewGravitySourceEvent OnNewGravitySource;

        /// <summary>
        ///     Used to add a new gravity source to the list. It'll fire an event to notify everyone
        /// </summary>
        /// <param name="gs"></param>
        public void AddGravitySource( GravitySource gs )
        {
            if (GravitySources == null) GravitySources = new List<GravitySource>();
            GravitySources.Add(gs);
            OnNewGravitySource?.Invoke(gs);
        }

        /// <summary>
        ///     Used to remove the gravity source from the list. It'll also trigger an event to notify everyone
        /// </summary>
        /// <param name="gs"></param>
        public void DestroyGravitySource( GravitySource gs )
        {
            GravitySources.Remove(gs);
            OnDestroyedGravitySource?.Invoke(gs);
        }
    }
}
