using UnityEngine;

public class LineTextureScroller : MonoBehaviour
{
    public Vector2 velocity;
    private Material lineMaterial;

    void Start()
    {
        lineMaterial = GetComponent<LineRenderer>().material;
    }

    void Update()
    {
        Vector2 newOffset = lineMaterial.GetTextureOffset("_MainTex") + velocity * Time.deltaTime;
        lineMaterial.SetTextureOffset("_MainTex", newOffset);
    }
}
