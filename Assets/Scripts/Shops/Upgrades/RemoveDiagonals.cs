public class RemoveDiagonals : BlockPaletteUpgrade
{

    public override Rarity rarity { get => Rarity.Common; }
    
    public override string name => "Chipped Ruler";

    public override string description() => "Remove all Diagonals from your current block palette.";

    protected override BlockDatabase ShapesToRemove => AssetLoader.Instance.DiagonalBlockPalette;
}
