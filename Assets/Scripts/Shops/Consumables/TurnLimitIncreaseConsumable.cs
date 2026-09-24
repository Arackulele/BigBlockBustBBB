using UnityEngine;

public class TurnLimitIncreaseConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Epic; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Golden Bread";

    public override string description => "Use to permanently gain 1 Turn, but loose 2 Consumable Slots";

    public override void OnDrop(Vector2 pos)
    {
        GameManager.Instance.TurnLimit++;
        GameManager.Instance.ChangeMaxConsumables(-2);
        UsedUp();
    }
}
