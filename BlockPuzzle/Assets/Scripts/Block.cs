using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Block : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer borderSpriteRenderer;
    private Animator animator;

    private Piece parent;
    public float spawnHeight;
    private Coroutine destroying;

    public Vector2Int Position { get { return new Vector2Int((int) transform.localPosition.x, (int) transform.localPosition.y); } }

    private BoxCollider2D GetCollider()
    {
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();

        return boxCollider;
    }

    private SpriteRenderer GetSpriteRenderer()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        return spriteRenderer;
    }

    private Material GetMaterial()
    {
        List<Material> mats = new List<Material>();
        transform.Find("Mesh").GetComponent<MeshRenderer>().GetMaterials(mats);

        return mats[0];
    }

    private SpriteRenderer GetBorderSpriteRenderer()
    {
        if (borderSpriteRenderer == null) borderSpriteRenderer = transform.Find("Glow").GetComponent<SpriteRenderer>();
        return borderSpriteRenderer;
    }

    private Animator GetAnimator()
    {
        if (animator == null) animator = GetComponent<Animator>();
        return animator;
    }

    public void SetParent(Piece piece)
    {
        parent = piece;
    }

    public bool CanMove(Vector2Int direction, BlockGroup group = null)
    {
        Vector2Int newPosition = Position + direction;

        if (!GameBoard.InBounds(newPosition)) return false;

        Block block = GameManager.main.board.GetBlockAt(newPosition);
        return !block || (group != null && group.Contains(block));
    }

    public void Move(Vector2Int direction)
    {
        transform.position = new Vector2(transform.position.x, transform.position.y) + direction;
    }

    public Bounds GetBounds()
    {
        return GetCollider().bounds;
    }

    public void SetColor(Color color)
    {
        GetSpriteRenderer().color = color;
        GetMaterial().SetColor("_Color", color);
    }

    public Color GetColor()
    {
        return GetSpriteRenderer().color;
    }

    public void SetBorderColor(Color color)
    {
        GetBorderSpriteRenderer().color = color;
    }

    public async Task DestroyAsync()
    {
        GetAnimator().Play("Destroying");

        await Task.Yield();

        while (GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            await Task.Yield();
        }

        Destroy();
    }

    public void Destroy()
    {
        enabled = false;
        Destroy(gameObject);
    }
}
