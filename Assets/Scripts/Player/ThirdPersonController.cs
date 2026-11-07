using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] CharacterController _charController;
    [SerializeField] Transform _camera;
    [SerializeField] InputActionReference controlMove;

    private Vector3 _direction;

    [SerializeField] float _speed;
    [SerializeField] float turnSmoothTime = 0f;

    private Vector3 _gravity = new Vector3(0f, -9.81f, 0f);
    [SerializeField] float _gravityMultiplier = 3.0f;
    private float _velocity = 0f;

    private float _turnSmoothVelocity;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector2 move = controlMove.action.ReadValue<Vector2>();

        _direction = new Vector3(move.x, 0, move.y).normalized;

        Debug.Log ("Mi dirección es " +  _direction);

        if (move.sqrMagnitude == 0) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _charController.Move(moveDirection.normalized * _speed * Time.deltaTime);
    }
}
