using UnityEngine;

namespace LaunchSystem
{
    public class LaunchSystemGraphicFeedback : MonoBehaviour
    {
        public LaunchSystem LaunchSystem;
        public LaunchSystemInput SystemInput;

        void Awake()
        {
            SystemInput.OnLaunchProcessStart
        }
    }
}
