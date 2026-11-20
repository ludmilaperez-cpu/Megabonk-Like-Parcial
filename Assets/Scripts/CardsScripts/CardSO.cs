using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public Sprite cardImage;

    public string cardText;


    public CardEffect effectType;
    public float effectValue;
    [Tooltip("Si esta marcado, effectValue es un porcentaje del valor actual del stat. Si no, se suma directo")]
    public bool isPercentage;

    public bool isUnique;
    public int unlockLevel;

    public PlayerStat Stat
    {
        get
        {
            switch (effectType)
            {
                case CardEffect.DamageIncrease: return PlayerStat.Damage;
                case CardEffect.HealthIncrease: return PlayerStat.MaxHealth;
                default: return PlayerStat.Speed;
            }
        }
    }
}

public enum CardEffect //efectos q puede tener la carta
{
    DamageIncrease,
    HealthIncrease,
    SpeedIncrease
}
