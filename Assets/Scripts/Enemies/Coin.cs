using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //solo el jugador puede recogerla
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Moneda recogida");

        //por ahora no suma monedas
        Destroy(gameObject);
    }
}
