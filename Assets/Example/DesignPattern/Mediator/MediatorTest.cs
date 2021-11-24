using UnityEngine;
using WithWhat.DesignPattern;

public class MediatorTest : MonoBehaviour
{
    public enum MediatorTestEnum
    { 
        Test,
    }

    private void Awake()
    {
        Mediator.Instance.Register(MediatorTestEnum.Test, func);       
    }

    private void func(int key, object[] param)
    {
        print($"{key} {param[0] as string}");
    }
}