using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Ataque")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Patrones de Ataque")]
    [SerializeField] private BossAttack[] attacks;

    private Transform player;
    private float attackTimer;

    public Transform Player => player;

    private void Start()
    {
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null)
            return;

        MoveToPlayer();

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            ExecuteRandomAttack();
        }
    }

    private void MoveToPlayer()
    {
        Vector3 direction =
            (player.position - transform.position).normalized;

        direction.y = 0f;

        transform.position +=
            direction *
            moveSpeed *
            Time.deltaTime;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
        }
    }

    private void ExecuteRandomAttack()
    {
        if (attacks == null || attacks.Length == 0)
        {
            Debug.LogWarning("Boss sin ataques asignados.");
            return;
        }

        int randomAttack =
            Random.Range(0, attacks.Length);

        if (attacks[randomAttack] == null)
        {
            Debug.LogError(
                "Ataque " + randomAttack + " no asignado en el Inspector.");
            return;
        }

        attacks[randomAttack].Execute(this);
    }

    public void SpawnProjectile(Vector3 targetPosition)
    {
        GameObject projectile =
            Instantiate(
                projectilePrefab,
                Vector3.zero,
                Quaternion.identity);

        BossProjectile proj =
            projectile.GetComponent<BossProjectile>();

        if (proj != null)
        {
            proj.SetTarget(targetPosition);
        }
    }

    private void OnDestroy()
    {
        BossSpawner spawner =
            FindObjectOfType<BossSpawner>();

        if (spawner != null)
        {
            spawner.BossDied();
        }
    }
}
