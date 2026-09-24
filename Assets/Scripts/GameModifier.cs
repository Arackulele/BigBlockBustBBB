using System.Collections.Generic;
using UnityEngine;

public abstract class GameModifier : Purchasable
{
    public virtual string name { get; }
    
    public virtual string description { get; }
    
    public virtual float LineClearModifier(List<Vector2Int> clearedBlocks) => 0;
    public virtual int BlockPlaceModifier(List<Vector2Int> placedPositions, UnplacedBlockScript block) => 0;
    public virtual float ShopPriceModifier(Purchasable item) => 0;
    public virtual float ShopBarModifier() => 0;
    public virtual int ComboGainMultiplier() => 0;
    public virtual void OnMultReset(int previousMult) { }
    public virtual void OnTurnPassed(int turn) { }
    public virtual float GlobalPointsMultiplier() => 0;
    public virtual void OnBlocksRefilled() { }
    public virtual void OnAdded() { }
    public virtual void OnRemoved() { }
    public virtual void AfterBlocksPlaced() { }
    public virtual void OnConsumableUsed() { }
    public virtual List<Vector2Int> GetAdditionalClearableBlocks(bool[,] gridMap) => new List<Vector2Int>();
}
