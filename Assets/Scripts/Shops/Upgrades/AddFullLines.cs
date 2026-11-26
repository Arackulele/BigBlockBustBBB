public class AddFullLines : BlockPaletteUpgrade
{

    public override Rarity rarity { get => Rarity.Epic; }
    
    public override string name => "Measuring Tape";

    public override string description() => "Adds a full row and column to your block palette.";

    protected override BlockDatabase ShapesToAdd => AssetLoader.Instance.FullLineBlockPalette;
}
