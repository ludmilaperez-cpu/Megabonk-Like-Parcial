using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PassiveItem : MonoBehaviour
{
    [SerializeField] protected ItemSO itemData;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null || itemData == null) return;

        ApplyModifier(player);
        Destroy(gameObject);
    }

    protected abstract void ApplyModifier(PlayerController player);
}
