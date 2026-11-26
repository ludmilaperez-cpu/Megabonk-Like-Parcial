using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] protected LayerMask _colliderLayer;
    [SerializeField] protected float _maxLifetime;
    [SerializeField] protected Transform _target;
    [SerializeField] protected float _hitRange;

    protected float _speed;
    protected float _damage;
    protected Vector3 _direction;

    protected virtual void Start()
    {
        StartCoroutine(Shoot(_direction, _speed, _maxLifetime));
    }

    protected virtual void Update()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _hitRange, _colliderLayer);

        var currentTarget = GetClosestEnemy(colliderArray);

        if (currentTarget != null)
        {
            if (currentTarget.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }

    protected Collider GetClosestEnemy(Collider[] enemies)
    {
        Vector3 currentPosition = transform.position;
        Collider bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float directionToTargetSqr = directionToTarget.sqrMagnitude;
            if (directionToTargetSqr < closestDistanceSqr)
            {
                closestDistanceSqr = directionToTargetSqr;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    protected virtual IEnumerator Shoot(Vector3 dir, float speed, float time)
    {
        float t = 0;

        while (t < time)
        {
            transform.position += dir.normalized * speed * Time.deltaTime;

            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }

    public void SetupStats(GunController gc)
    {
        _speed = gc.speed;
        _damage = gc.CurrentDamage;
    }

    public void SetupDirection(Vector3 dir)
    {
        _direction = dir;

        transform.forward = dir;
    }

    public void SetupTarget(Transform target)
    {
        _target = target;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blanchedAlmond;
        Gizmos.DrawSphere(transform.position, _hitRange);
    }
}
