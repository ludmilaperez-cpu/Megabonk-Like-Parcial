using ScreenManager;
using UnityEngine;

namespace ScreenManager 
{
    [RequireComponent(typeof(Canvas))]
    public class SimpleUiScreen : MonoBehaviour, IScreen
    {
        private Canvas _canvas;
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;
        }
        public void Activate()
        {
            _canvas.enabled = true;
        }
        public void Deactivate()
        {
            _canvas.enabled = false;
        }
        public void Release()
        {
            _canvas.enabled = false;
        }
    }
}