using UnityEngine;

public class BlockDiscardConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Common; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBlockArea; }


    public override string name => "Mint leaf";

    public override string description => "Use to discard a block";

    public override void OnDrop(Vector2 pos)
    {
        if (BlockPlacementArea.instance != null &&
            BlockPlacementArea.instance.DiscardBlockAt(pos))
        {
            UsedUp();
        }
    }

    public override void OnTurnPassed(int turn)
    {
        if (GameManager.Instance.HasConsumable<GainMultConsumable>())
        GameManager.Instance.AddConsumable(new GainMultConsumable());
    }
}
