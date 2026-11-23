using UnityEngine;
using UnityEngine.Rendering;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject prefab;
    public float damage, speed, cooldown, scale, spawnInterval, knockback;
    public int quantity, pierce;

    protected float currentCooldown, currentSpawnInterval;

    protected virtual void Start()
    {
        currentCooldown = cooldown;
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = cooldown;
    }

}
