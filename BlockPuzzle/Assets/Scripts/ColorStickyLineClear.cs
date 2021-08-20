using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStickyLineClear : StickyLineClear
{
    protected override bool ShouldGroupBlock(Block pivot, Block newBlock)
    {
        return newBlock != null && newBlock.GetColor() == pivot.GetColor();
    }
}
