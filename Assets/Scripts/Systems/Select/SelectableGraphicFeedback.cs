using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace SelectSystem
{
    /// <summary>
    /// This class takes care of showing when an item has been selected
    /// </summary>
    public class SelectableGraphicFeedback : MonoBehaviour
    {
        public Selectable Selectable;
        public Light Light;

        public Color IdleColor;
        public Color SelectedColor;

        void Awake()
        {
            Assert.IsNotNull(Light);
            Selectable.OnSelectedStatusChanged += OnSelectedStatusChanged;
        }

        void OnDestroy()
        {
            Selectable.OnSelectedStatusChanged -= OnSelectedStatusChanged;
        }

        /// <summary>
        ///     Changes the color of the lights indicating whether an item it's selected or not
        /// </summary>
        /// <param name="oldstatus"></param>
        /// <param name="newstatus"></param>
        void OnSelectedStatusChanged( Selectable.StatusEnum oldstatus, Selectable.StatusEnum newstatus )
        {
            switch (newstatus)
            {
                case Selectable.StatusEnum.Selected:
                    Light.color = SelectedColor;
                    break;
                case Selectable.StatusEnum.Idle:
                    Light.color = IdleColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newstatus), newstatus, null);
            }
        }
    }
}
