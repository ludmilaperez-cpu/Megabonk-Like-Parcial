using System.Collections.Generic;
using UnityEngine;

namespace ScreenManager 
{
    public class ScreenManager : MonoBehaviour
    {
        private static ScreenManager _instance;
        public static ScreenManager Instance 
        {
            get
            {
                if(_instance == null) 
                {
                    _instance = FindFirstObjectByType<ScreenManager>();
                }
                return _instance;
            }
        }
        private Stack<IScreen> _screenStack;

        /// <summary>Cuantas pantallas hay apiladas (la base cuenta como 1).</summary>
        public int Count => _screenStack.Count;

        private void Awake()
        {
            _screenStack = new Stack<IScreen>();
        }
        public void Push (IScreen newScreen) 
        {
            if (_screenStack.Contains(newScreen)) return;
            if(_screenStack.Count !=0)
            {
                var oldScreen = _screenStack.Peek();
                oldScreen.Deactivate();
            }
            _screenStack.Push(newScreen);
            newScreen.Activate();
        }
        public void Pop() 
        {
            if (_screenStack.Count <= 1) return;
            var screenToPop = _screenStack.Pop();
            screenToPop.Release();

            var lastScreen = _screenStack.Peek();
            lastScreen.Activate();
        }
    }
}