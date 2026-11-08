using UnityEngine;



#region Strategy
public interface IEnemyStrategy //Strategy, define comportamiento generico que ejecuta el enemigo

{
    void Execute(Enemy enemy);
}

public class ChaseStrategy : IEnemyStrategy //estrategia de persecucion. hace que el enemigo se mueva hacia el jugador.

{
    public void Execute(Enemy enemy)
    {
        enemy.Move();
    }
}

public class AttackStrategy : IEnemyStrategy // de ataque
{
    public void Execute(Enemy enemy)
    {
        enemy.AttackBehaviour();
    }
}
#endregion
public class Enemy : MonoBehaviour
{

    [Header("Flyweight - Shared Enemy Data")] //flyweight, los datos compartidos por los enemigos se almacenan en un ScriptableObject para evitar duplicar info
                                              
    [SerializeField] private EnemyData enemyData;

    [Header("Look Up Table")]
    [SerializeField] private int enemyLevel = 0;

    private int[] damageTable = //Look Up Table, almacena el daño segun el nivel del enemigo.
    {
        10,
        15,
        20,
        30,
        50
    };

    private Transform player;
    private float attackTimer;
    private int damage;

    private IEnemyStrategy currentStrategy;

    private ChaseStrategy chaseStrategy;
    private AttackStrategy attackStrategy;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); //busca al player

        if (playerObj != null)
            player = playerObj.transform;

        chaseStrategy = new ChaseStrategy(); //inicia estrategias enemigos
        attackStrategy = new AttackStrategy();

        damage = damageTable[
            Mathf.Clamp(enemyLevel, 0, damageTable.Length - 1) //daño segun el lvl de la tabla
        ];

        currentStrategy = chaseStrategy;
    }

    void Update()
    {
        if (player == null)
            return;

        UpdateStrategy(); //que comportamiento usar?

        currentStrategy?.Execute(this);
    }

    void UpdateStrategy()
    {
        float distance = Vector3.Distance( //si el jugador esta cerca ataca si esta lejos lo persigue
            transform.position,
            player.position);

        if (distance <= enemyData.attackRange)
            currentStrategy = attackStrategy;
        else
            currentStrategy = chaseStrategy;
    }

    public void Move()
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
            enemyData.moveSpeed *
            Time.deltaTime;

        if (finalDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(finalDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                enemyData.rotationSpeed * Time.deltaTime);
        }
    }

    public void AttackBehaviour()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= enemyData.attackCooldown)
        {
            attackTimer = 0f;

            Debug.Log("Enemy attacks for " + damage);

            // player.GetComponent<PlayerHealth>()
            //      .TakeDamage(damage);
        }
    }

    Vector3 GetSeparationForce()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(
            transform.position,
            enemyData.separationRadius);

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

            if (distance < enemyData.personalSpace)
            {
                separation +=
                    diff.normalized *
                    (enemyData.personalSpace - distance);
            }
        }

        return separation * enemyData.separationStrength;
    }

    void OnDrawGizmosSelected()
    {
        if (enemyData == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            enemyData.separationRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            enemyData.personalSpace);
    }

    private void OnDestroy()
    {
        WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();

        if (waveSpawner != null &&
            waveSpawner.currentWaveIndex < waveSpawner.waves.Length)
        {
            waveSpawner.waves[
                waveSpawner.currentWaveIndex
            ].enemiesLeft--;
        }
    }
}
