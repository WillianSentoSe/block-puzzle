using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class NaiveLineClear : LineClearBase
{
    public override async Task ClearLine(GameBoard board, int row)
    {
        await base.ClearLine(board, row);

        for (int rowAbove = row; rowAbove < GameBoard.height; rowAbove++)
        {
            for (int collumn = 0; collumn < GameBoard.width; collumn++)
            {
                Block block = board.GetBlockAt(new Vector2Int(collumn, rowAbove));
                if (block) block.Move(Vector2Int.down);
            }
        }
    }
}
