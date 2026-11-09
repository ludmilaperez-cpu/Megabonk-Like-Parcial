using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour
{
    [Header("Impacto")]
    [SerializeField] private float fallTime = 0.05f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage = 25;
    [Header("Referencias")]
    [SerializeField] private GameObject warningPrefab;

    private Renderer[] renderers;
    private Vector3 targetPosition;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        //oculta el proyectil al instanciarse
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;

        StartCoroutine(FallAttack());
    }

    private IEnumerator FallAttack()
    {
        //marca roja en el suelo
        GameObject warning = Instantiate(
            warningPrefab,targetPosition + Vector3.up * 0.05f, Quaternion.Euler(90f, 0f, 0f));

        yield return new WaitForSeconds(0.6f);

        Destroy(warning);

        //aparece en el cielo
        transform.position = targetPosition + Vector3.up * 10f;

        //ahora se vuelve visible
        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }

        while (Vector3.Distance(transform.position, targetPosition) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPosition,40f * Time.deltaTime);

            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Boss hit player");

                // hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
}