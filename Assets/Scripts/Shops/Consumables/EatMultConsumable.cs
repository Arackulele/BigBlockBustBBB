using UnityEngine;

public class EatMultConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Tomatillo";

    public override string description => "Use to reset your Combo, gain 7 Money for each Combo point removed";

    public override void OnDrop(Vector2 pos)
    {
        ScoreManagement.Instance.UnspentScore += ScoreManagement.Instance.Combo * 7;
        ScoreManagement.Instance.DepleteMult();
        UsedUp();
    }
}
