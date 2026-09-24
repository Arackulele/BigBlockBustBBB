using UnityEngine;

public class GainScoreConsumable : Consumable
{
    public override Rarity rarity { get => Rarity.Common; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Cubed Melon";

    public override string description => "Use to get 1D6*10 Score, affected by mult";

    public override void OnDrop(Vector2 pos)
    {
        ScoreManagement.Instance.AddScore(Random.Range(1, 6) * 10);
        UsedUp();
    }
}
