using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Chase,
        Attack
    }

    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 10f;

    [Header("Ataque")]
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int damage = 10;

    [Header("Steering")]
    [SerializeField] float separationRadius = 1.5f;
    [SerializeField] float separationStrength = 3f;
    [SerializeField] float personalSpace = 0.8f;

    private Transform player;
    private EnemyState currentState;
    private float attackTimer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

        currentState = EnemyState.Chase;
    }

    void Update()
    {
        if (player == null)
            return;

        UpdateState();
        Move();
        AttackBehaviour();
    }

    void UpdateState()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.position);

        currentState = distance <= attackRange
            ? EnemyState.Attack
            : EnemyState.Chase;
    }

    void Move()
    {
        Vector3 seekDirection =
            (player.position - transform.position).normalized;

        Vector3 separationDirection =
            GetSeparationForce();

        Vector3 finalDirection =
            (seekDirection + separationDirection).normalized;

        finalDirection.y = 0f;

        transform.position +=
            finalDirection *
            moveSpeed *
            Time.deltaTime;

        if (finalDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(finalDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    void AttackBehaviour()
    {
        if (currentState != EnemyState.Attack)
            return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            Debug.Log("Enemy attacks");

            // player.GetComponent<PlayerHealth>()
            //      .TakeDamage(damage);
        }
    }

    Vector3 GetSeparationForce()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(
            transform.position,
            separationRadius);

        Vector3 separation = Vector3.zero;

        foreach (Collider col in nearbyEnemies)
        {
            if (col.gameObject == gameObject)
                continue;

            if (!col.CompareTag("Enemy"))
                continue;

            Vector3 diff =
                transform.position -
                col.transform.position;

            diff.y = 0f;

            float distance = diff.magnitude;

            if (distance <= 0.001f)
                continue;

            if (distance < personalSpace)
            {
                separation +=
                    diff.normalized *
                    (personalSpace - distance);
            }
        }

        return separation * separationStrength;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            separationRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            personalSpace);
    }

    private void OnDestroy()
    {
        FindObjectOfType<WaveSpawner>()
            .waves[
                FindObjectOfType<WaveSpawner>().currentWaveIndex
            ]
            .enemiesLeft--;
    }
}

