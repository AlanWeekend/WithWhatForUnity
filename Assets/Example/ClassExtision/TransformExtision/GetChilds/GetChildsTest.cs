using UnityEngine;
using WithWhat.ClassExtision;

public class GetChildsTest : MonoBehaviour
{
    void Start()
    {
        foreach (var child in this.transform.GetChilds())
        {
            print(child.name);
        }
    }
}