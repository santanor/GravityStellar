using System.Collections.Generic;
using Models;
using SelectSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime_sets.SelectedDots
{
    [RequireComponent(typeof(Dot))]
    [RequireComponent(typeof(Selectable))]
    public class SelectedDotRuntimeSetAgent : MonoBehaviour
    {
        public SelectedDotsSet SelectedDots;
        [SerializeField] Selectable selectable;
        [SerializeField] Dot dot;

        void Awake()
        {
            Assert.IsNotNull(SelectedDots);
            selectable.OnSelectedStatusChanged += StatusChanged;
        }

        /// <summary>
        /// Gets called when a selected status changed, adding it or removing it from the
        /// selected dots runtime set
        /// </summary>
        /// <param name="oldstatus"></param>
        /// <param name="newstatus"></param>
        void StatusChanged( Selectable.StatusEnum oldstatus, Selectable.StatusEnum newstatus )
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (oldstatus == Selectable.StatusEnum.Idle && newstatus == Selectable.StatusEnum.Selected)
            {
                SelectedDots.AddNonDup(new KeyValuePair<Dot, Selectable>(dot, selectable));
            }

            if (oldstatus == Selectable.StatusEnum.Selected && newstatus == Selectable.StatusEnum.Idle)
            {
                SelectedDots.Remove(new KeyValuePair<Dot, Selectable>(dot, selectable));
            }
        }
    }
}
