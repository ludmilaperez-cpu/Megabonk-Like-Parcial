public class SpeedBoostItem : PassiveItem
{
    protected override void ApplyModifier(PlayerController player)
    {
        player.Speed += itemData.modifierValue;
    }
}
