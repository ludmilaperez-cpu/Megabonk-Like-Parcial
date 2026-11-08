using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Movimiento")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;

    [Header("Ataque")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    [Header("Steering")]
    public float separationRadius = 1.5f;
    public float separationStrength = 3f;
    public float personalSpace = 0.8f;
}
