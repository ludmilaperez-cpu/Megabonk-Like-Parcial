using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    [SerializeField] CharacterController _charController;
    private Vector3 _direction;
    [SerializeField] Transform _camera;

    [SerializeField] float _rotationSpeed = 500f;
    private Camera _mainCamera;

    [SerializeField] float _speed;

    private float _gravity = -9.81f;
    [SerializeField] float _gravityMultiplier;
    private float _velocity;

    [SerializeField] float _jumpPower;
    private int _jumpCount;
    [SerializeField] int _maxJumpCount;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0f)
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        _direction = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * new Vector3(_input.x, 0f, _input.y);
        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        _charController.Move(_direction * _speed * Time.deltaTime );
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        _direction = new Vector3(_input.x, 0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!IsGrounded() && _jumpCount >= _maxJumpCount) return;
        if (_jumpCount == 0) StartCoroutine(WaitForLanding());

        _jumpCount++;

        _velocity = _jumpPower;
        //_velocity = _jumpPower / _jumpCount;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        _jumpCount = 0;
    }

    private bool IsGrounded() => _charController.isGrounded;
}
