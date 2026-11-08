using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float spawnRadius = 20f;

    public Wave[] waves;
    public int currentWaveIndex = 0;

    private float countdown;
    private bool readyToCountDown;

    private void Start()
    {
        readyToCountDown = true;

        countdown = 1f;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemyCount;
        }
    }

    private void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("You survived every wave!");
            return;
        }

        if (readyToCountDown)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;

            StartCoroutine(SpawnWave());

            countdown = waves[currentWaveIndex].timeToNextWave;
        }

        if (waves[currentWaveIndex].enemiesLeft <= 0)
        {
            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                readyToCountDown = true;
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        Wave wave = waves[currentWaveIndex];

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Vector3 spawnPos = GetSpawnPositionOutsideView();

            Instantiate(
                wave.enemyPrefab,
                spawnPos,
                Quaternion.identity
            );

            yield return new WaitForSeconds(
                wave.timeToNextEnemy
            );
        }
    }

    private Vector3 GetSpawnPositionOutsideView()
    {
        Vector3 spawnPosition;

        do
        {
            Vector2 randomCircle =
                Random.insideUnitCircle.normalized *
                spawnRadius;

            spawnPosition = player.position +
                new Vector3(
                    randomCircle.x,
                    0,
                    randomCircle.y);
        }
        while (IsVisible(spawnPosition));

        return spawnPosition;
    }

    private bool IsVisible(Vector3 worldPosition)
    {
        Vector3 viewportPos =
            mainCamera.WorldToViewportPoint(worldPosition);

        return viewportPos.z > 0 &&
               viewportPos.x > 0 &&
               viewportPos.x < 1 &&
               viewportPos.y > 0 &&
               viewportPos.y < 1;
    }
}

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;

    public int enemyCount = 10;

    public float timeToNextEnemy = 1f;

    public float timeToNextWave = 5f;

    [HideInInspector]
    public int enemiesLeft;
}