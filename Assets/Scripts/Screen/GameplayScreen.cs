using UnityEngine;
using System.Collections.Generic;
namespace ScreenManager
{
    public class GameplayScreen : MonoBehaviour, IScreen
    {
        List<Behaviour> _previousStates;
        private void Awake()
        {
            _previousStates = new List<Behaviour>();
        }

        public void Activate() 
        {
            foreach (var behavior in _previousStates)
            {
                behavior.enabled = true;
            }
            _previousStates.Clear();
           
        }
        public void Deactivate() 
        {
            foreach (var behaviour in GetComponentsInChildren<Behaviour>()) 
            {
                if(behaviour == this) continue;
                if(behaviour is Camera or AudioListener) continue;
                if(!behaviour.enabled) continue;

                _previousStates.Add(behaviour);
                behaviour.enabled = false;

            }
        }
        public void Release() 
        {
        
        }

    }
}