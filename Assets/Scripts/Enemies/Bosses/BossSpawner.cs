using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] bosses;

    [SerializeField] private Transform player;

    [SerializeField] private float spawnRadius = 20f;

    [SerializeField] private float bossInterval = 120f;

    private float timer;

    private int bossIndex;

    private GameObject currentBoss;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= bossInterval)
        {
            timer = 0f;

            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        //si ya existe un boss vivo no spawneamos otro
        if (currentBoss != null)
            return;

        if (bossIndex >= bosses.Length)
            bossIndex = 0;

        Vector3 spawnPos =
            player.position +
            Random.onUnitSphere * spawnRadius;

        spawnPos.y = player.position.y;

        currentBoss =
            Instantiate(
                bosses[bossIndex],
                spawnPos,
                Quaternion.identity);

        bossIndex++;
    }

    public void BossDied()
    {
        currentBoss = null;
    }
}
