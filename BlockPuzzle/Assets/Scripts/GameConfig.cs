
using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Game")]
public class GameConfig : ScriptableObject
{
    [Header("Falling Speed")]
    public float initialFallSpeed = 2f;
    public float acceleration = 0.02f;
    public float fallingSpeed;

    [Header("Spawn")]
    public Piece[] pieces;
    public Color[] colors;
    public float spawnDelay = 0.5f;

    public void Restore()
    {
        fallingSpeed = initialFallSpeed;
    }

    public Piece GetPiece()
    {
        float count = 0;

        foreach(var piece in pieces)
        {
            if (piece.spawnWeight >= 0)
            {
                count += piece.spawnWeight;
            }
        }

        float randomValue = Random.Range(0f, count);

        foreach (var piece in pieces)
        {
            if (piece.spawnWeight >= 0 && piece.spawnWeight >= randomValue)
            {
                return piece;
            }
            else
            {
                randomValue -= piece.spawnWeight;
            }
        }

        return null;
    }

    public Color GetColor()
    {
        return colors[Random.Range(0, colors.Length)];
    }

    public void UpdateSpeed()
    {
        fallingSpeed += acceleration;
    }
}
