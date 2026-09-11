using UnityEngine;

public class ShopItem : Purchasable
{
    [SerializeField]
    protected Sprite sprite;

    public virtual Rarity rarity { get; }
    
    public virtual string name { get; }
    
    public virtual string description { get; }
    
    public virtual float baseprice { get; }


    public override double price()
    {
        double Price = baseprice + UnityEngine.Random.Range(2, 10);
        
        switch (rarity)
        {
            case Rarity.Common: Price *= 1; break;
            case Rarity.Rare:   Price *= 3;  break;
            case Rarity.Epic:   Price *= 6;  break;
        }
        
        return Price;
    }
}

    
    
public enum Rarity
{
    Common,
    Rare,
    Epic
}