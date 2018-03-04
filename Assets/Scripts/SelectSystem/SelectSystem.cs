using System.Collections.Generic;
using InputSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace SelectSystem
{
    public class SelectSystem : ScriptableObject
    {
        enum StatusEnum {Idle, Selecting}

        StatusEnum _status;
        public InputManager InputManager;
        [SerializeField] IList<Selectable> _selectables;
        Bounds _bounds;

        void Awake()
        {
            _selectables = new List<Selectable>();
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);

            InputManager.OnDrag += OnDrag;
            InputManager.OnDragFinish += OnDragFinish;
            InputManager.OnTouch += OnTouch;
        }

        public void AddSelectable( Selectable s )
        {
            _selectables.Add(s);
        }

        public void RemoveSelectable( Selectable s )
        {
            _selectables.Remove(s);
        }

        void OnTouch( Vector2 screenpos, Vector2 worldpos )
        {

        }

        void OnDragFinish( Vector2 screenpos, Vector2 worldpo )
        {

        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {

        }
    }

    public static class MakeScriptableObject
    {
        [MenuItem("Assets/Create/SelectSystem")]
        public static void CreateMyAsset()
        {
            var asset = ScriptableObject.CreateInstance<SelectSystem>();

            AssetDatabase.CreateAsset(asset, "Assets/Scripts/SelectSystem/SelectSytem.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
