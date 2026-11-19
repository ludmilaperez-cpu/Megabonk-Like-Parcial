using UnityEngine;
namespace ScreenManager
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigibodyExample : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private Vector3 _currentVelocity;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            _rigidbody.isKinematic = false;

            _rigidbody.linearVelocity = _currentVelocity;   
        }
        private void OnDisable()
        {
            _currentVelocity = _rigidbody.linearVelocity;
            _rigidbody.isKinematic = true;
        }
    }
}