using UnityEngine;
using WithWhat.DesignPattern;

public class Foo : MonoSingleTon<Foo>
{
    public void DoSomething()
    {
        print("do it");
    }
}

public class MonoSingleTonTest : MonoBehaviour
{
    void Start()
    {
        Foo.Instance.DoSomething();       
    }
}