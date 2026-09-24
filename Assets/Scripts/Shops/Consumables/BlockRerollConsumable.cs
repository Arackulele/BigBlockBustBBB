using UnityEngine;

public class BlockRerollConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Common; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.OnBlockArea; }


    public override string name => "Breath Mint";

    public override string description => "Use to reroll placeable blocks";

    public override void OnDrop(Vector2 pos)
    {
        BlockPlacementArea.instance.FillBlocks(true);
        UsedUp();
    }
}
