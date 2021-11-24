using UnityEngine;
using WithWhat.DesignPattern;

public class InjectTest : MonoBehaviour
{
    public class Foo {
        public void Do() { print($"{GetHashCode()}"); }
    }

    private void Awake()
    {
        ServiceLocator.RegisterSingleTon<Foo>();
    }

    void Start()
    {
        var foo = ServiceLocator.Resolve<Foo>();
        foo.Do();
    }
}
