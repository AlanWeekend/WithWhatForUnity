using UnityEngine;
using WithWhat.DesignPattern;

public class TransientFactoryTest : MonoBehaviour
{
    private TransientObjectFactory _transientObjectFactory;

    public class Foo 
    {
        public void Do() { print($"{GetHashCode()}:DoSomething"); }
    }

    private void Awake()
    {
        _transientObjectFactory = new TransientObjectFactory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var foo = _transientObjectFactory.AcquireObject<Foo>();
            foo.Do();
        }
    }
}
