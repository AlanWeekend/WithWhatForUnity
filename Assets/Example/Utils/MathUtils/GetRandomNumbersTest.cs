using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WithWhat.Utils;

public class GetRandomNumbersTest : MonoBehaviour
{
    private void Awake()
    {
        var lst = MathUtils.GetRandomNumbers(10,1,15,false,new List<int>() { 1,3,4});
        Debug.Log(string.Join(",", lst));
    }
}