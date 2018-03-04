using UnityEngine;

namespace SelectSystem
{
    public class Selectable : MonoBehaviour
    {
        public enum StatusEnum
        {
            Idle,
            Selected
        }

        public delegate void SelectedStatusChanged( StatusEnum oldStatus, StatusEnum newStatus );

        public SelectSystem SelectSystem;
        public StatusEnum Status;
        public SelectedStatusChanged OnSelectedStatusChanged;


    }
}
