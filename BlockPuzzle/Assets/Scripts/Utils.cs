using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Vector2Int Snap(Vector2 position, float unitSize = GameBoard.unitSize)
    {
        int x = Mathf.RoundToInt(position.x - unitSize / 2);
        int y = Mathf.RoundToInt(position.y - unitSize / 2);

        return new Vector2Int(x, y);
    }

    public static Vector2Int RotateAround(Vector2 vector, Vector2 pivot, float angle)
    {
        Vector2 delta = vector - pivot;
        Vector2 newPosition = Quaternion.Euler(0, 0, angle) * delta;

        int x = Mathf.RoundToInt(newPosition.x + pivot.x);
        int y = Mathf.RoundToInt(newPosition.y + pivot.y);

        return new Vector2Int(x, y);


    }

    public static Vector2Int ToVector2Int(Vector2 vector)
    {
        int x = Mathf.RoundToInt(vector.x);
        int y = Mathf.RoundToInt(vector.y);

        return new Vector2Int(x, y);
    }

    public static string SetZerosColor(string text, string color)
    {
        int i = 0;
        while (i < text.Length && text[i++] == '0') ;

        if (i == 0) return text;
        else if (i == text.Length) return "<color=" + color + ">" + text + "</color>";
        else return i > 0 ? "<color=" + color + ">" + text.Insert(i - 1, "</color>") : text;
    }
}
