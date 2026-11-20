public class LifeBoostItem : PassiveItem
{
    protected override void ApplyModifier(PlayerController player)
    {
        player.MaxHealth += itemData.modifierValue;
        player.CurrentHealth += itemData.modifierValue;
    }
}
