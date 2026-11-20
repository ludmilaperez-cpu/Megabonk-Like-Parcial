using UnityEngine;

/// <summary>Stat del player al que le suma un upgrade.</summary>
public enum PlayerStat
{
    MaxHealth,
    Speed,
    Damage
}

/// <summary>
/// Definicion de un upgrade comprable en la store. Los assets tienen que estar en
/// Assets/Resources/Upgrades para que el UpgradeManager los encuentre.
/// </summary>
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

    [Tooltip("N: precio de la primera compra")]
    public float basePrice = 50f;

    [Tooltip("Cuanto se multiplica el precio en cada compra (1.2 = +20%)")]
    public float priceMultiplier = 1.2f;
}
