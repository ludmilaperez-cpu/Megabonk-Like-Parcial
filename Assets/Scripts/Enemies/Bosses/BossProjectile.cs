using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour
{
    [Header("Impacto")]
    [SerializeField] private float fallTime = 0.5f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage = 25;

    [Header("Referencias")]
    [SerializeField] private GameObject warningPrefab;

    private Vector3 targetPosition;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;

        StartCoroutine(FallAttack());
    }

    private IEnumerator FallAttack()
    {
        //marca roja de ataq
        GameObject warning = Instantiate(
            warningPrefab,
            targetPosition + Vector3.up * 0.05f,
            Quaternion.identity);

        yield return new WaitForSeconds(fallTime);

        Destroy(warning);

        //proyectil cae desde el cielo
        transform.position =
            targetPosition + Vector3.up * 10f;

        while (Vector3.Distance(transform.position, targetPosition) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                20f * Time.deltaTime);

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
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius);
    }
}
