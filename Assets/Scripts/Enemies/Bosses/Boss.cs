using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Ataque")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int projectileCount = 5;

    private Transform player;
    private float attackTimer;

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
            ShootVolley();
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

    private void ShootVolley()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projectile =
                Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.identity);

            BossProjectile proj =
                projectile.GetComponent<BossProjectile>();

            proj.SetTarget(player.position);
        }
    }
}
