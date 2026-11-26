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

    public float Speed { get; set; }

    [SerializeField] float _maxHealth = 100f; 
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    [SerializeField] float _damage = 10f;
    public float Damage { get; set; }

    public void ResetStats()
    {
        Speed = _speed;
        MaxHealth = _maxHealth;
        Damage = _damage;
    }

    public float GetStat(PlayerStat stat)
    {
        switch (stat)
        {
            case PlayerStat.MaxHealth: return MaxHealth;
            case PlayerStat.Speed: return Speed;
            case PlayerStat.Damage: return Damage;
            default: return 0f;
        }
    }

    public void ModifyStat(PlayerStat stat, float value, bool isPercentage = false)
    {
        float delta = isPercentage ? GetStat(stat) * value / 100f : value;

        switch (stat)
        {
            case PlayerStat.MaxHealth:
                MaxHealth += delta;
                CurrentHealth += delta;
                break;
            case PlayerStat.Speed: Speed += delta; break;
            case PlayerStat.Damage: Damage += delta; break;
        }
    }

    public bool IsDead { get; private set; }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        if (CurrentHealth <= 0f) Die();
    }

    private void Die()
    {
        IsDead = true;
        if (GameManager.instance != null)
            GameManager.instance.ChangeState(GameManager.GameState.GameOver);
    }

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
        ResetStats();
        UpgradeManager.ApplyTo(this); 
        CurrentHealth = MaxHealth;
    }

    // Debug temporal muestra los stats actuales arriba a la izquierda (borrar cuando haya UI de vida)
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 400, 25), $"Vida: {CurrentHealth} / {MaxHealth}");
        GUI.Label(new Rect(10, 30, 400, 25), $"Velocidad: {Speed}");
        GUI.Label(new Rect(10, 50, 400, 25), $"Daño: {Damage}");
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
        _charController.Move(_direction * Speed * Time.deltaTime);
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
