using UnityEngine;

public class MeteorAttack : BossAttack
{
    [SerializeField] private int meteorCount = 8;

    [SerializeField] private float minRadius = 2f;
    [SerializeField] private float maxRadius = 6f;

    public override void Execute(Boss boss)
    {
        Vector3 playerPosition = boss.Player.position;

        for (int i = 0; i < meteorCount; i++)
        {
            Vector2 randomCircle =
                Random.insideUnitCircle.normalized *
                Random.Range(minRadius, maxRadius);

            Vector3 targetPosition =
                playerPosition +
                new Vector3(
                    randomCircle.x,
                    0f,
                    randomCircle.y);

            boss.SpawnProjectile(targetPosition);
        }

        Debug.Log("Meteor Attack");
    }
}
