using UnityEngine;
using WithWhat.ClassExtision;

public class SetTheWorldPostionToTheMousePositionTest : MonoBehaviour
{
    public GameObject plan;

    void Update()
    {
        this.transform.SetPostionToMousePosition(Camera.main, plan.transform.position, Vector3.zero);
    }
}