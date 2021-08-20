using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

public abstract class LineClearBase {

    public virtual async Task ClearLine(GameBoard board, int row)
    {
        List<Task> tasks = new List<Task>();

        await Task.Delay(100);

        for (int collumn = 0; collumn < GameBoard.width; collumn++)
        {
            Block block = board.GetBlockAt(new Vector2Int(collumn, row));
            if (block) tasks.Add(board.DestroyBlockAsync(block));

            await Task.Delay(100);
        }

        await Task.WhenAll(tasks.ToArray());
    }
}
