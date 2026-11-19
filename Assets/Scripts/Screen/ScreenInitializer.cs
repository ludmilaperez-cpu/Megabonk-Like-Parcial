using UnityEngine;

namespace ScreenManager
{
    public class ScreenInitializer : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _initialScreen;

        private void Start()
        {
            if (_initialScreen is IScreen screen)
                ScreenManager.Instance.Push(screen);
            else
                Debug.LogError("[ScreenInitializer] _initialScreen no esta asignado o no implementa IScreen", this);
        }
    }
}
