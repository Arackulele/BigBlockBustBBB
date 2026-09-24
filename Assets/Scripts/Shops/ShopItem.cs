using UnityEngine;

public class ShopItem : GameModifier
{
    [SerializeField]
    protected Sprite sprite;

    private double? cachedPrice;

    public virtual Rarity rarity { get; }
    
    public virtual float baseprice { get; }


    public override double price()
    {
        if (cachedPrice.HasValue)
            return cachedPrice.Value;

        double generatedPrice = baseprice + UnityEngine.Random.Range(2, 10);
        
        switch (rarity)
        {
            case Rarity.Common: generatedPrice *= 1;  break;
            case Rarity.Rare:   generatedPrice *= 3;  break;
            case Rarity.Epic:   generatedPrice *= 6;  break;
        }
        
        cachedPrice = generatedPrice;
        return cachedPrice.Value;
    }

    public void ResetPrice()
    {
        cachedPrice = null;
    }
}

    
    
public enum Rarity
{
    Common,
    Rare,
    Epic
}
