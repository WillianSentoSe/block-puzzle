using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : List<Block>
{
    public BlockGroup(Block[] blocks) : base(blocks) { }
    public BlockGroup() : base() { }

    public void Move(Vector2Int direction)
    {
        ForEach(block => block.Move(direction));
    }

    public bool CanMoveTo(Vector2Int direction)
    {
        return !Exists(block => !block.CanMove(direction, this));
    }

    public bool MoveToBottom()
    {
        bool hasMoved = false;

        while (CanMoveTo(Vector2Int.down)) {
            hasMoved = true;
            Move(Vector2Int.down);
        }

        return hasMoved;
    }
}
