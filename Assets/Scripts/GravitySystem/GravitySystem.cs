using System.Collections.Generic;
using UnityEditor;
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

        /// <summary>
        /// Used to add a new gravity source to the list. It'll fire an event to notify everyone
        /// </summary>
        /// <param name="gs"></param>
        public void AddGravitySource( GravitySource gs )
        {
            GravitySources.Add(gs);
            OnNewGravitySource?.Invoke(gs);
        }

        /// <summary>
        /// Used to remove the gravity source from the list. It'll also trigger an event to notify everyone
        /// </summary>
        /// <param name="gs"></param>
        public void DestroyGravitySource( GravitySource gs )
        {
            GravitySources.Remove(gs);
            OnDestroyedGravitySource?.Invoke(gs);
        }

    }

    public static class MakeScriptableObject {
        [MenuItem("Assets/Create/GravitySystem")]
        public static void CreateMyAsset()
        {
            var asset = ScriptableObject.CreateInstance<GravitySystem>();

            AssetDatabase.CreateAsset(asset, "Assets/Scripts/GravitySystem/GravitySytem.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
