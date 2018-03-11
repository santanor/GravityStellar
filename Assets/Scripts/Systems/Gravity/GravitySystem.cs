using System.Collections.Generic;
using UnityEngine;

namespace GravitySystem
{
    public class GravitySystem : ScriptableObject
    {
        public delegate void GravitySourceEvent( GravitySource gs );

        public delegate void GravityReceiverEvent( GravityReceiver gr );

        public IList<GravitySource> GravitySources;
        public IList<GravityReceiver> GravityReceivers;
        public GravitySourceEvent OnDestroyedGravitySource;
        public GravitySourceEvent OnNewGravitySource;
        public GravityReceiverEvent OnNewGravityReceiver;
        public GravityReceiverEvent OnDestroyedGravityReceiver;

        /// <summary>
        /// Used to add a gravity receiver to the list. It also triggers an event
        /// </summary>
        /// <param name="gr"></param>
        public void AddGravityReceiver( GravityReceiver gr )
        {
            if (GravityReceivers == null) GravityReceivers = new List<GravityReceiver>();
            GravityReceivers.Add(gr);
            OnNewGravityReceiver?.Invoke(gr);
        }

        /// <summary>
        /// Used to remove a gravity receiver from the list. It also trigger an event
        /// </summary>
        /// <param name="gr"></param>
        public void RemoveGravityReceiver( GravityReceiver gr )
        {
            OnDestroyedGravityReceiver?.Invoke(gr);
            GravityReceivers.Remove(gr);
        }

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
            OnDestroyedGravitySource?.Invoke(gs);
            GravitySources.Remove(gs);
        }
    }
}
