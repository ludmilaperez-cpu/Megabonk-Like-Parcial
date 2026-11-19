using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int minValue = 1;
    [SerializeField] int maxValue = 10;   

    private int value;

    private void Awake()
    {
        value = Random.Range(minValue, maxValue + 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Player"))
            return;

        var collector = other.GetComponentInParent<PlayerCollector>();
        if (collector == null) collector = other.gameObject.AddComponent<PlayerCollector>();
        collector.AddCoins(value);

        Destroy(gameObject);
    }
}
