using UnityEngine;
using WithWhat.ClassExtision;

public class WorldToCanvasPointTest : MonoBehaviour
{
    public GameObject Cube;
    public Canvas canvas;

    void Start()
    {
        print(RectTransformExtision.WorldToCanvasPoint(canvas, Cube.transform.position));
    }
}