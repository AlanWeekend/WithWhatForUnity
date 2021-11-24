using UnityEngine;
using WithWhat.DesignPattern;

public class SingleTonFactoryTest : MonoBehaviour
{
    private SingleTonObejctFactory _singleTonObejctFactory;

    public class HttpService
    {
        public void GetData() { print($"{this.GetHashCode()}:GetData"); }
    }

    private void Awake()
    {
        _singleTonObejctFactory = new SingleTonObejctFactory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var httpService = _singleTonObejctFactory.AcquireObject<HttpService>();
            httpService.GetData();
        }
    }
}
