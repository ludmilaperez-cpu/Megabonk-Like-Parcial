using UnityEngine;

public class FocusAttack : BossAttack
{
    [SerializeField] private int projectileCount = 6;
    [SerializeField] private float spacing = 2f;

    public override void Execute(Boss boss)
    {
        Vector3 playerPos = boss.Player.position;

        Vector3 direction =
            (playerPos - boss.transform.position).normalized;

        direction.y = 0f;

        for (int i = 1; i <= projectileCount; i++)
        {
            Vector3 targetPos =
                boss.transform.position +
                direction * spacing * i;

            boss.SpawnProjectile(targetPos);
        }

        Debug.Log("Focus Attack");
    }
}
