using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Upgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Lunchbox";

    public override string description => "A random Tile on the Board is secretly marked, placing a block on said tile will grant 3 random Consumables and removes this upgrade";

    private Vector2Int SecretPos;

    public override int BlockPlaceModifier(List<Vector2Int> PlacedPositions, UnplacedBlockScript Block)
    {
        if (PlacedPositions.Contains(SecretPos))
        {
            Debug.Log("Treasure found");
            for (int i = 0; i < 4; i++) GameManager.Instance.AddConsumable(ShopManager.instance.consumableShop.SelectConsumable());
            Remove();
        }
        
        return 0;
    }

    public override void OnAdded()
    {
        SecretPos = GameBoard.instance.GetRandomPosition();
        Debug.Log("Treasure position is: "  + SecretPos.ToString());
    }
}
