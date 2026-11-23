using Mono.Cecil.Cil;
using System.Threading;
using UnityEngine;

public class TargettingSystem : MonoBehaviour
{
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] float _range;
    [SerializeField] Color _color;
    public Transform target;

    /* !!!!!!!!!!!!!!!!! PENDIENTE !!!!!!!!!!!!!!!!!
    Agregar un ligero delay al chequeo de qué enemigo está más cerca dentro del rango para que no haga la cuenta absolutamente
    todos los frames así está todo mejor optimizado y no es tan evil :3 */

    private void Update()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _range, _enemyLayer);

        var currentTarget = GetClosestEnemy(colliderArray);

        if (currentTarget != null)
        {
            target = currentTarget.transform;
        }
        else if (GetClosestEnemy(colliderArray) == null)
        {
            target = null;
        }
    }

    Collider GetClosestEnemy(Collider[] enemies)
    {
        Vector3 currentPosition = transform.position;
        Collider bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider potentialTarget in enemies)
        {
            if (potentialTarget.TryGetComponent<Enemy>(out Enemy enemy))
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float directionToTargetSqr = directionToTarget.sqrMagnitude;
                if (directionToTargetSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = directionToTargetSqr;
                    bestTarget = potentialTarget;

                    Debug.Log("Encontré a " + bestTarget + " como mejor target");
                }
            }
        }

        return bestTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _range);

        if (target == null) return;
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, target.transform.position - transform.position);
        }
    }

}
