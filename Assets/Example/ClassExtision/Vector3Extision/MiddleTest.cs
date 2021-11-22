using UnityEngine;
using WithWhat.ClassExtision;

public class MiddleTest : MonoBehaviour
{
    public GameObject other;

    void Start()
    {
        print(this.transform.position.Middle(other.transform.position));
    }
}