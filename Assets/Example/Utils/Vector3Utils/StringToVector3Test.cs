using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WithWhat.Utils;

public class StringToVector3Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector3Utils.StringToVector3("(1,2,3)");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
