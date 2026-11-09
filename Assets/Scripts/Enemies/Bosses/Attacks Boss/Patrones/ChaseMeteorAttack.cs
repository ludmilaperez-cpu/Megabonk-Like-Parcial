using UnityEngine;
using System.Collections;

public class ChaseMeteorAttack : BossAttack
{
    [SerializeField] private int meteorCount = 10;
    [SerializeField] private float delayBetweenMeteors = 0.15f;

    public override void Execute(Boss boss)
    {
        StartCoroutine(AttackRoutine(boss));
    }

    private IEnumerator AttackRoutine(Boss boss)
    {
        for (int i = 0; i < meteorCount; i++)
        {
            Vector3 targetPosition =
                boss.Player.position;

            boss.SpawnProjectile(targetPosition);

            yield return new WaitForSeconds(delayBetweenMeteors);
        }
    }
}