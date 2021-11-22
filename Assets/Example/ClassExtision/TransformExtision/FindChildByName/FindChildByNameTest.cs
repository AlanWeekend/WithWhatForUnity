using UnityEngine;
using WithWhat.ClassExtision;

public class FindChildByNameTest : MonoBehaviour
{
    void Start()
    {
        print(this.transform.FindChildByName("GameObject").name);      
    }
}