using UnityEngine;
using WithWhat.ClassExtision;

public class GetMousePositionTest : MonoBehaviour
{
    void Update()
    {
        print((this.transform as RectTransform).GetMousePosition());
    }
}