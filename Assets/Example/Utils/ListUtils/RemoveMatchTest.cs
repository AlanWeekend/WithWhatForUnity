using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WithWhat.Utils;

public class RemoveMatchTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var lst = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        lst.RemoveMatch(i => i > 5);
        foreach (var item in lst)
        {
            print(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
