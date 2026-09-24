using UnityEngine;

public class PermaMultConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Epic; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Golden Salad";

    public override string description => "Use to get 5% more points for the rest of the run";

    public override void OnDrop(Vector2 pos)
    {
        GameManager.Instance.GlobalMultiplier += 0.05;
        UsedUp();
    }
}
