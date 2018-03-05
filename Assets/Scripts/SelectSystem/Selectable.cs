using UnityEngine;

namespace SelectSystem
{
    public class Selectable : MonoBehaviour
    {
        public delegate void SelectedStatusChanged( StatusEnum oldStatus, StatusEnum newStatus );

        public enum StatusEnum
        {
            Idle,
            Selected
        }

        public SelectedStatusChanged OnSelectedStatusChanged;

        public SelectSystem SelectSystem;
        public StatusEnum Status;

        void Awake()
        {
            SelectSystem = SelectSystem == null ? FindObjectOfType<SelectSystem>() : SelectSystem;
            SelectSystem.AddSelectable(this);
        }

        void OnDestroy()
        {
            SelectSystem.RemoveSelectable(this);
        }

        public void Select()
        {
            OnSelectedStatusChanged?.Invoke(Status, StatusEnum.Selected);
            Status = StatusEnum.Selected;
        }

        public void Deselect()
        {
            OnSelectedStatusChanged?.Invoke(Status, StatusEnum.Idle);
            Status = StatusEnum.Idle;
        }
    }
}
