using UnityEngine;
using WithWhat.DesignPattern;

public class Foo1 : Singleton<Foo1>
{
    private Foo1() { }

    public void DoSomething()
    {
        Debug.Log("i do");
    }
}

public class SingleTonTest : MonoBehaviour
{
    void Start()
    {
        Foo1.Instance.DoSomething();
    }
}
