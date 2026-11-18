using UnityEngine;

public class GainScoreConsumableBig : Consumable
{
    public override Rarity rarity { get => Rarity.Rare; }

    public override ConsumableVisual.PlacementType Placement { get => ConsumableVisual.PlacementType.Anywhere; }


    public override string name => "Peeled Kiwi";

    public override string description => "Use to get 1D12*20 Score, affected by mult";

    public override void OnDrop(Vector2 pos)
    {
        ScoreManagement.Instance.AddScore(Random.Range(1, 12) * 20);
        UsedUp();
    }
}
