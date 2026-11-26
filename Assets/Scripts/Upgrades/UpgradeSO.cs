using UnityEngine;

public enum PlayerStat
{
    MaxHealth,
    Speed,
    Damage,
    AttackSpeed
}

[CreateAssetMenu(fileName = "UpgradeNuevo", menuName = "Upgrades/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    [Tooltip("Clave con la que se guarda en SaveData.json (ej: UpgradeLife). No cambiarla despues.")]
    public string id = "UpgradeLife";
    public string displayName = "+ Vida";

    [Tooltip("Stat del player al que se le suma")]
    public PlayerStat stat;

    [Tooltip("X: cuanto suma al stat cada compra")]
    public float valuePerLevel = 10f;

    [Tooltip("Si esta marcado, X es un porcentaje del stat base (25 = +25% por compra) en vez de un valor fijo")]
    public bool isPercentage;

    [Tooltip("N: precio de la primera compra")]
    public float basePrice = 50f;

    [Tooltip("Cuanto se multiplica el precio en cada compra (1.2 = +20%)")]
    public float priceMultiplier = 1.2f;
}
