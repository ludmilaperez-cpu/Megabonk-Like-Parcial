using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbingProjectile : BulletController
{
    [SerializeField] float _angle;
    [SerializeField] float _explosionRange;
    
    private Vector3 _startPos;
    private bool _drawExplosion;

    protected override void Start()
    {
        _startPos = transform.position;

        if (_target == null) return;
        float angle = _angle * Mathf.Deg2Rad;

        Vector3 groundDirection = new Vector3(_direction.x, 0, _direction.z);

        Vector3 targetPosition = new Vector3(groundDirection.magnitude, _direction.y, 0);

        float v0;
        float time;

        CalculatePath(targetPosition, angle, out v0, out time);

        StartCoroutine(CoroutineMovement(groundDirection.normalized, v0, angle, _maxLifetime));
    }

    protected override void Update()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _hitRange, _colliderLayer);

        var currentTarget = GetClosestEnemy(colliderArray);

        if (currentTarget != null)
        {
            _drawExplosion = true;

            //Agregar feedback al instanciar particulas o algo :v

            Collider[] enemies = Physics.OverlapSphere(transform.position, _explosionRange, _colliderLayer);

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].TryGetComponent<Enemy>(out Enemy enemy)) enemy.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }

    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float gravity = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * gravity;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    IEnumerator CoroutineMovement(Vector3 dir, float v0, float angle, float time)
    {
        float t = 0;

        while (t < time)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _startPos + dir * x + Vector3.up * y;

            // Quizá para influenciar la velocidad de la bala convenga poner un multiplicador basado en la velocidad de disparo del player?
            t += Time.deltaTime * (1 + (_speed / 10));

            yield return null;
        }

        yield return null;
    }

    protected void OnDrawGizmos()
    {
        if (_drawExplosion)
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawSphere(transform.position, _explosionRange);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.hotPink;
        Gizmos.DrawSphere(transform.position, _explosionRange);

    }
}
