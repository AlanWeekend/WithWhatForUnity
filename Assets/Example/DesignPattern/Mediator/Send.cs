using UnityEngine;
using WithWhat.DesignPattern;

public class Send : MonoBehaviour
{
    void Start()
    {
        Mediator.Instance.Send(MediatorTest.MediatorTestEnum.Test, "hello");       
    }
}
