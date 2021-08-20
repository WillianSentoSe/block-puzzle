using UnityEngine;

public class Piece : MonoBehaviour
{
    #region Properties

    public static Color activeBorderColor = new Color(0.8f, 0.7f, 0.8f, 1f);
    public static Color defaultBorderColor = new Color(0.12f, 0.12f, 0.12f);
    private const float rotationAngle = -90;

    public float fallSpeed = 2f;
    public float spawnWeight = 1f;
    public Vector2 rotationPivot;

    private Vector2Int position;
    private BlockGroup blocks;
    private float verticalDelay = 0f;
    private float speedMultiplier = 1f;

    private Color color;

    public GameObject blockProjectionPrefab;
    private GameObject pieceProjection;

    public System.Action<Piece> onReachBottom;

    public Color Color { get { return color; } set { SetColor(value); } }

    #endregion

    #region Execution

    public void OnEnable()
    {
        blocks = GetBlocks();
    }

    public void Start()
    {
        CreateProjection();
    }

    public void Update()
    {
        verticalDelay -= Time.deltaTime * speedMultiplier;

        while (verticalDelay <= 0) {
            verticalDelay += 1 / GameManager.main.configurations.fallingSpeed;
            Move(0, -1);
        }

        UpdatePosition(true);
    }

    #endregion

    #region Public Methods

    public void SetPosition(Vector2Int newPosition)
    {
        position = newPosition;
        UpdatePosition(false);
    }

    public void UpdatePosition(bool smooth = false)
    {
        if (smooth == false)
        {
            transform.position = GameBoard.GetWorldPoint(position);
        }
        else
        {
            // Vector3 delta = (Vector3) GameBoard.GetWorldPoint(position) - transform.position;
            // transform.position += delta.normalized * Time.deltaTime * (GameManager.main.configurations.fallingSpeed * 5f * speedMultiplier);
            
            transform.position = GameBoard.GetWorldPoint(position);
        }
    }

    public void Move(int xDistance, int yDistance)
    {
        Vector2Int newPosition = position + new Vector2Int(xDistance, yDistance);

        if (blocks.CanMoveTo(newPosition))
        {
            position = newPosition;
        }
        else if (yDistance < 0)
        {
            Destroy(pieceProjection);
            onReachBottom?.Invoke(this);
        }

        UpdateProjectionPosition();

    }

    public void SetLayer(int layer)
    {
        gameObject.layer = layer;

        foreach (var block in blocks)
        {
            block.gameObject.layer = layer;
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void Rotate(float angle = rotationAngle)
    {
        if (CanRotate(angle))
        {
            foreach (var block in blocks)
            {
                Vector2 newPosition = Utils.RotateAround(block.transform.localPosition, rotationPivot, angle);
                block.transform.localPosition = newPosition;
            }
        }

        if (pieceProjection)
        {
            Destroy(pieceProjection);
            CreateProjection();
        }
    }

    private bool CanRotate(float angle = rotationAngle)
    {
        foreach (var block in blocks)
        {
            Vector2Int newPosition = Utils.RotateAround(block.transform.localPosition, rotationPivot, angle);
            
            if (!GameBoard.InBounds(position + newPosition) || GameBoard.CheckPosition(position + newPosition))
                return false;
        }

        return true;
    }

    private void SetColor(Color color)
    {
        this.color = color;

        foreach (var block in blocks)
        {
            block.SetColor(color);
        }

        if (pieceProjection)
        {
            foreach (var sr in pieceProjection.transform.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = color;
            }
        }
    }

    public void SetBorderColor(Color color)
    {
        foreach (var block in blocks)
        {
            block.SetBorderColor(color);
        }
    }

    public Bounds GetBounds()
    {
        Bounds bounds = new Bounds();

        foreach (var block in GetBlocks())
        {
            bounds.Encapsulate(block.GetBounds());
        }

        return bounds;
    }

    public BlockGroup GetBlocks()
    {
        if (blocks == null)
        {
            blocks = new BlockGroup(transform.GetComponentsInChildren<Block>());

            foreach (var block in blocks)
            {
                block.SetParent(this);
            }
        }

        return blocks;
    }

    #endregion

    private void CreateProjection()
    {
        pieceProjection = new GameObject(name + " Projection");
        pieceProjection.transform.position = transform.position;
        pieceProjection.transform.SetParent(transform);

        blocks.ForEach(block =>
        {
            GameObject blockProjection = Instantiate(blockProjectionPrefab, pieceProjection.transform);
            blockProjection.transform.localPosition = block.transform.localPosition;
        });

        UpdateProjectionPosition();
    }

    private void UpdateProjectionPosition()
    {
        if (!pieceProjection) return;

        int distance = 0;

        while (blocks.CanMoveTo(position + new Vector2Int(0, distance)))
        {
            distance -= 1;
        }

        pieceProjection.transform.localPosition = new Vector3(0f, distance + 1, 0f);
    }


    #region Misc

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, GameBoard.GetWorldPoint(position));
        Gizmos.DrawIcon((Vector2) transform.position + rotationPivot + new Vector2(.5f, .5f) * GameBoard.unitSize, "center.png");
    } 

    #endregion
}
