using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : WeaponController
{
    public bool quantityUpdated;
    [SerializeField] TargettingSystem _targettingSystem;
    [SerializeField] PlayerController _playerController;

    protected override void Start()
    {
        base.Start();

        if (quantity > 1)
        {
            quantityUpdated = true;
            Debug.Log("El player empezó con " + quantity + " de cantidad");
        }
    }

    protected override void Update()
    {
        currentCooldown -= Time.deltaTime;

        /* Se necesita actualizar la condición que chequea este if para que esté vinculada con cuando el player actualiza sus stats,
        ahora mismo está con un placeholder para hacer una prueba de concepto */
        if (quantityUpdated)
        {
            currentSpawnInterval = spawnInterval / quantity;
            Debug.Log("El intervalo de spawn es " + currentSpawnInterval);
            quantityUpdated = false;
        }

        if (currentCooldown <= 0 && _targettingSystem.target != null)
        {
            Attack();
            Shoot(currentSpawnInterval, quantity);
        }
    }

    // Daño del arma multiplicado por las mejoras de daño del player (store y cartas)
    public float CurrentDamage => _playerController != null ? damage * _playerController.DamageMultiplier : damage;

    // Velocidad de ataque del player: acorta el cooldown entre disparos
    protected override void Attack()
    {
        base.Attack();
        if (_playerController != null && _playerController.AttackSpeed > 0f)
            currentCooldown = cooldown / _playerController.AttackSpeed;
    }

    protected void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    IEnumerator MultipleShooting(float time, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnBullet();

            yield return new WaitForSeconds(time);
        }
    }

    protected void Shoot(float time, int amount)
    {
        if (amount > 1)
        {
            StartCoroutine(MultipleShooting(time, amount));
        }
        else
        {
            SpawnBullet();
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(prefab, transform.position, Quaternion.identity);

        bullet.GetComponent<BulletController>().SetupStats(this);

        if (_targettingSystem.target != null)
        {
            Vector3 dir = transform.rotation * (_targettingSystem.target.position - transform.position);

            bullet.GetComponent<BulletController>().SetupDirection(dir);

            if (_targettingSystem.target != null)
            {
                Debug.Log(_targettingSystem.target);
                bullet.GetComponent<BulletController>().SetupTarget(_targettingSystem.target);

            }
        }
    }
}
