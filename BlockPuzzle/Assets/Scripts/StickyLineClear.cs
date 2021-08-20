using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StickyLineClear : LineClearBase
{
    public override async Task ClearLine(GameBoard board, int row)
    {
        await base.ClearLine(board, row);

        List<Block> blocks = new List<Block>(board.GetBlocks()).FindAll(block => block.Position.y > row);
        List<BlockGroup> groups = new List<BlockGroup>();
        bool hasMoved = true;

        while (blocks.Count > 0)
        {
            Block block = blocks[0];
            BlockGroup group = GroupNeighbourhood(board, block);
            groups.Add(group);
            blocks.RemoveAll(_block => group.Contains(_block));
        }

        while (hasMoved)
        {
            hasMoved = false;
            groups.ForEach(group => { if (group.MoveToBottom()) hasMoved = true; });
        }
    }

    public BlockGroup GroupNeighbourhood(GameBoard board, Block block, BlockGroup group = null) {

        if (group == null) group = new BlockGroup();

        group.Add(block);

        Block top = board.GetBlockAt(block.Position + Vector2Int.up);
        if (ShouldGroupBlock(block, top) && !group.Contains(top)) group = GroupNeighbourhood(board, top, group);

        Block right = board.GetBlockAt(block.Position + Vector2Int.right);
        if (ShouldGroupBlock(block, right) && !group.Contains(right)) group = GroupNeighbourhood(board, right, group);

        Block bottom = board.GetBlockAt(block.Position + Vector2Int.down);
        if (ShouldGroupBlock(block, bottom) && !group.Contains(bottom)) group = GroupNeighbourhood(board, bottom, group);

        Block left = board.GetBlockAt(block.Position + Vector2Int.left);
        if (ShouldGroupBlock(block, left) && !group.Contains(left)) group = GroupNeighbourhood(board, left, group);

        return group;
    }

    protected virtual bool ShouldGroupBlock(Block pivot, Block newBlock)
    {
        return newBlock != null;
    }
}