using System.Collections.Generic;
using UnityEngine;

public class TetrominoBlocks : Upgrade
{

    public override Rarity rarity { get => Rarity.Rare; }
    
    public override string name => "Gameboy Cartridge";

    public override string description => "Replace your block palette with the set of Tetrominos";

    public override void OnAdded()
    {
        BlockPlacementArea.instance.ChangeDatabase(AssetLoader.Instance.TetrominoBlockPalette);
        OnActivate();
    }

    public override void OnRemoved()
    {
        //ToDo: This should check other placement changing upgrades before always reverting to default
        BlockPlacementArea.instance.ChangeDatabase(AssetLoader.Instance.DefaultBlockPalette);
    }
}