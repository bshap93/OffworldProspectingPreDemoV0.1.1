using UnityEngine;

namespace PixelCrushers.CompassNavigatorProDemo
{
    public class LockCursorUtility : MonoBehaviour
    {
        public bool lockOnStart = true;

        void Start()
        {
            if (lockOnStart) SetCursorLock(true);
        }

        public void SetCursorLock(bool lockCursor)
        {
            Cursor.visible = !lockCursor;
            Cursor.lockState = lockCursor  ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}