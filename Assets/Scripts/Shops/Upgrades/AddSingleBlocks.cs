public class AddSingleBlocks : BlockPaletteUpgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Ping Pong Paddles";

    public override string description() => "Adds two single blocks to your block palette.";

    protected override BlockDatabase ShapesToAdd => AssetLoader.Instance.SingleBlockPalette;
}
