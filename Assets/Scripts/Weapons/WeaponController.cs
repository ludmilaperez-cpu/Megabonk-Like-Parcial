using UnityEngine;
using UnityEngine.Rendering;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject prefab;
    public float damage;
    public float speed;
    public float cooldown;
    public int pierce;
    public float scale; 

    float currentCooldown;

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
