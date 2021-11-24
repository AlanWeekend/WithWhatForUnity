using UnityEngine;
using WithWhat.DesignPattern;

public class PoolFactoryTest : MonoBehaviour
{
    private PoolObjectFactory _poolObjectFactory;

    public class DBConnect {
        public void Insert() { print($"insert{this.GetHashCode()}"); }
    }

    private void Awake()
    {
        _poolObjectFactory = new PoolObjectFactory(100, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 从对象池获取对象
            var dbConnect = _poolObjectFactory.AcquireObject<DBConnect>();
            dbConnect.Insert();
            // 使用完，返还给对象池
            _poolObjectFactory.ReleaseObject(dbConnect);
        }
    }
}
