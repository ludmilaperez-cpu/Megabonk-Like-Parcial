using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{ //crear enemigos cerca del player y fuera de la camara
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float spawnRadius = 20f;

    public Wave[] waves;
    public int currentWaveIndex = 0;

    private float countdown;
    private bool readyToCountDown;

    private void Start() //inicia oleada con enemigos
    {
        readyToCountDown = true;

        countdown = 1f;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemyCount;
        }
    }

    private void Update() //si ya no hay oleadas termina el sist
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("You survived every wave");
            return;
        }

        if (readyToCountDown)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0) //nueva oleada
        {
            readyToCountDown = false;

            StartCoroutine(SpawnWave());

            countdown = waves[currentWaveIndex].timeToNextWave;
        }

        if (waves[currentWaveIndex].enemiesLeft <= 0) //comprueba si los enemigos fueron eliminados
        {
            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                readyToCountDown = true;
            }
        }
    }

    private IEnumerator SpawnWave() //oleada actual y genera enemigos de oleada, busca punto fuera de la camara 
    {
        Wave wave = waves[currentWaveIndex];

        for (int i = 0; i < wave.enemyCount; i++) 
        {
            Vector3 spawnPos = GetSpawnPositionOutsideView();

            Instantiate(wave.enemyPrefab,spawnPos,Quaternion.identity);

            yield return new WaitForSeconds(wave.timeToNextEnemy);
        }
    }

    private Vector3 GetSpawnPositionOutsideView() //sigue generando puntos aleatorios hasta encontrar uno q no este en pantalla
    {
        Vector3 spawnPosition;

        do
        {
            Vector2 randomCircle =Random.insideUnitCircle.normalized *spawnRadius;

            spawnPosition = player.position + new Vector3(randomCircle.x,0,randomCircle.y);
        }
        while (IsVisible(spawnPosition));

        return spawnPosition;
    }

    private bool IsVisible(Vector3 worldPosition)
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(worldPosition); //comprueba si esta dentro de la pantallas

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