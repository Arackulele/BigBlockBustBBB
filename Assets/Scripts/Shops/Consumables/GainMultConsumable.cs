using UnityEngine;

public class GainMultConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Common; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Cherry Tomato";

    public override string description => "Use to get 1 Combo, and extend your Combo by 1 Round";

    public override void OnDrop(Vector2 pos)
    {
        ScoreManagement.Instance.IncrementMult();
        UsedUp();
    }
}
