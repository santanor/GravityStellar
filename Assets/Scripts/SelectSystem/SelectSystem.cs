using System.Collections.Generic;
using InputSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace SelectSystem
{
    public class SelectSystem : ScriptableObject
    {
        [SerializeField] IList<Selectable> _selectables;

        StatusEnum _status;
        public Bounds Bounds;

        //Used to store the first drag position.
        public Vector2 FirstPos;
        public Vector2 FourthPos;
        public InputManager InputManager;
        public Vector2 SecondPos;
        public Vector2 ThirdPos;

        void OnEnable()
        {

            _selectables = new List<Selectable>();
            InputManager = InputManager == null ? FindObjectOfType<InputManager>() : InputManager;
            Assert.IsNotNull(InputManager);

            InputManager.OnDrag += OnDrag;
            InputManager.OnDragFinish += OnDragFinish;
            InputManager.OnTouch += OnTouch;
        }

        void OnDisable()
        {
            InputManager.OnDrag -= OnDrag;
            InputManager.OnDragFinish -= OnDragFinish;
            InputManager.OnTouch -= OnTouch;
        }

        /// <summary>
        ///     Called From each selectable to subscribe itself to the current system
        /// </summary>
        /// <param name="s"></param>
        public void AddSelectable( Selectable s )
        {
            if(_selectables == null) _selectables = new List<Selectable>();
            _selectables.Add(s);
        }

        /// <summary>
        ///     Called From each selectable to delete itself to the current system
        /// </summary>
        /// <param name="s"></param>
        public void RemoveSelectable( Selectable s )
        {
            _selectables.Remove(s);
        }

        void OnTouch( Vector2 screenpos, Vector2 worldpos )
        {
            RestartSelecteds();
            FirstPos = worldpos;
        }

        void OnDragFinish( Vector2 screenpos, Vector2 worldpo )
        {
            _status = StatusEnum.Idle;
        }

        void OnDrag( Vector2 screenpos, Vector2 worldpo )
        {
            _status = StatusEnum.Selecting;
            Bounds = new Bounds();
            CalculateBoundCorners(worldpo);

            //Encapsulate the corners
            Bounds.Encapsulate(FirstPos);
            Bounds.Encapsulate(SecondPos);
            Bounds.Encapsulate(ThirdPos);
            Bounds.Encapsulate(FourthPos);

            SelectWithinBounds();
        }

        void SelectWithinBounds()
        {
            foreach (var selectable in _selectables)
            {
                if (Bounds.Contains(selectable.transform.position))
                {
                    selectable.Select();
                }
                else
                {
                    selectable.Deselect();
                }
            }
        }

        /// <summary>
        ///     Restart all the selectables to 'idle'
        /// </summary>
        void RestartSelecteds()
        {
            foreach (var selectable in _selectables) selectable.Deselect();
        }

        /// <summary>
        ///     Assigns the corners of the bounds creating a square from the first finger pos and the current one
        /// </summary>
        void CalculateBoundCorners( Vector2 currentFingerPos )
        {
            SecondPos = new Vector2(FirstPos.x, currentFingerPos.y);
            ThirdPos = new Vector2(currentFingerPos.x, FirstPos.y);
            FourthPos = currentFingerPos;
        }

        enum StatusEnum
        {
            Idle,
            Selecting
        }
    }

    #region Create Asset

    /// <summary>
    ///     Only used to create the asset
    /// </summary>
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

    #endregion
}
