using UnityEngine;

public class ComboAddConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Heirloom Tomato";

    public override string description => "Use to double your combo";

    public override void OnDrop(Vector2 pos)
    {
        ScoreManagement.Instance.Combo *= 2;
        GameBoard.instance.GettingMult = true;
        UsedUp();
    }
}
