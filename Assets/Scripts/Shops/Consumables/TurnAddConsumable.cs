using UnityEngine;

public class TurnAddConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Common; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Croissant";

    public override string description() => "Use to gain 1 Turn";

    public override void OnDrop(Vector2 pos)
    {
        LevelProgression.instance.CurrentTurn--;
        UsedUp();
    }
}
