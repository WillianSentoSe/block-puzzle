using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    #region Properties

    public const float unitSize = 1f;
    public const int width = 10;
    public const int height = 20;

    private Transform blocksTransform;
    private LineClearBase lineClearController;
    private LineClearTypes lineClearMode;

    #endregion

    #region Getters and Setters

    public Transform BlocksTransform { 
        get
        {
            if (blocksTransform == null) blocksTransform = transform.Find("Blocks");
            return blocksTransform;
        } 
    }

    public int MaxHeight
    {
        get
        {
            int maxHeight = 0;

            foreach (var block in FindObjectsOfType<Block>())
            {
                if (block.gameObject.layer == 8)
                {
                    maxHeight = Mathf.Max(maxHeight, (int)Utils.Snap(block.transform.position).y);
                }
            }

            return maxHeight + 1;
        }
    }

    public LineClearTypes LineClearMode
    {
        get
        {
            return lineClearMode;
        }

        set
        {
            SetLineClearMode(value);
        }
    }

    #endregion

    public void Start()
    {
        lineClearController = new NaiveLineClear();
    }

    #region Static Methods

    public static bool InBounds(Vector2Int position)
    {
        return (
            position.x >= 0 &&
            position.y >= 0 &&
            position.x < width
        );
    }

    public static bool CheckPosition(Vector2Int position)
    {
        return Physics2D.OverlapPoint(GetWorldPoint(position) + new Vector2(0.5f, 0.5f), LayerMask.GetMask("Blocks")) != null;
    }

    public static Vector2 GetWorldPoint(Vector2Int position)
    {
        return new Vector2(position.x, position.y) * unitSize;
    }

    #endregion

    #region Public Methods

    public Block GetBlockAt(Vector2Int position) {
        foreach (var block in GetBlocks())
            if (block.enabled && Utils.Snap(block.transform.position + new Vector3(0.5f, 0.5f)) == position) 
                return block;

        return null;
    }

    public void Clear()
    {
        Block[] blocks = GetBlocks();

        for (int i = blocks.Length - 1; i >= 0; i--)
        {
            DestroyBlock(blocks[i]);
        }
    }

    public Piece SpawnPiece(Piece piecePrefab)
    {
        Piece newPiece = Instantiate(piecePrefab);
        Vector2Int spawnPosition = GetRandomSpawnPosition(newPiece.GetBounds());
        newPiece.SetPosition(spawnPosition);

        return newPiece;
    }

    public void DismantlePiece(Piece piece)
    {
        foreach (var block in piece.GetBlocks())
        {
            block.transform.SetParent(BlocksTransform);
        }

        Destroy(piece.gameObject);
    }

    [ContextMenu("Check Rows")]
    public async Task CheckRows(System.Action onLineCleared)
    {
        for (int row = 0; row < height; row++)
        {
            int collumn;

            for (collumn = 0; collumn < width; collumn++)
            {
                if (!GetBlockAt(new Vector2Int(collumn, row))) {
                    break;
                }
            }

            if (collumn == width)
            {
                await lineClearController.ClearLine(this, row);

                if (onLineCleared != null) onLineCleared.Invoke();
                row = -1;
            }
        }
    }

    public async Task DestroyBlockAsync(Block block)
    {
        await block.DestroyAsync();
    }

    public void DestroyBlock(Block block)
    {
        block.Destroy();
    }

    public Block[] GetBlocks()
    {
        return BlocksTransform.GetComponentsInChildren<Block>(false);
    }

    public void SetLineClearMode(LineClearTypes lineClearMode)
    {
        this.lineClearMode = lineClearMode;

        switch (this.lineClearMode)
        {
            case LineClearTypes.Naive:
                lineClearController = new NaiveLineClear();
                break;

            case LineClearTypes.Sticky:
                lineClearController = new StickyLineClear();
                break;

            case LineClearTypes.ColorSticky:
                lineClearController = new ColorStickyLineClear();
                break;
        }
    }

    #endregion

    #region Private Methods

    private Vector2Int GetRandomSpawnPosition(Bounds bounds)
    {
        int x = Random.Range((int) (bounds.size.x / unitSize / 2), (int) (width - (bounds.size.x / unitSize / 2)));
        int y = height;

        return new Vector2Int(x, y);
    }

    #endregion

    #region Misc

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(width / 2, height / 2) * unitSize, new Vector2(width, height) * unitSize);
    }

    #endregion

    public enum LineClearTypes
    {
        Naive, Sticky, ColorSticky
    }
}
